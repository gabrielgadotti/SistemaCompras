using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; } = new List<Item>();
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor, int condicaoPagamento)
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            CondicaoPagamento = new CondicaoPagamento(condicaoPagamento);
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
            if (!itens?.Any() ?? false)
                throw new BusinessRuleException("A solicitação de compra deve possuir itens!");

            foreach (var item in itens)
                Itens.Add(item);

            TotalGeral = new Money(itens.Select(x => x.Subtotal.Value).Sum());
            if (TotalGeral.Value > 50000)
                CondicaoPagamento = new CondicaoPagamento(30);

            AddEvent(new CompraRegistradaEvent(Id, Itens, TotalGeral.Value));
        }
    }
}
