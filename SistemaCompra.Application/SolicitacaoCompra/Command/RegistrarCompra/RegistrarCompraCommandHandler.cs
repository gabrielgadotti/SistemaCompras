using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand, bool>
    {
        private readonly ISolicitacaoCompraRepository _solicitacaoCompraRepository;
        private readonly IProdutoRepository _produtoRepository;

        public RegistrarCompraCommandHandler(ISolicitacaoCompraRepository solicitacaoCompraRepository, IProdutoRepository produtoRepository, IUnitOfWork uow, IMediator mediator) : base(uow, mediator)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidOperationException($"Falha ao processar comando.");

            var solicitacaoCompra = new SolicitacaoCompraAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor, request.CondicaoPagamento);

            var itens = new List<Item>(request.Itens.Count());
            foreach (var item in request.Itens)
            {
                var produto = await _produtoRepository.ObterAsync(item.ProdutoId, cancellationToken);
                itens.Add(new Item(produto, item.Quantidade));
            }

            solicitacaoCompra.RegistrarCompra(itens);

            _solicitacaoCompraRepository.RegistrarCompra(solicitacaoCompra);

            if (!Commit())
                return false;

            PublishEvents(solicitacaoCompra.Events);

            return true;
        }
    }
}
