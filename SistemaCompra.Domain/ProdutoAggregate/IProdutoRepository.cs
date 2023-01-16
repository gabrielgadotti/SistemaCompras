using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaCompra.Domain.ProdutoAggregate
{
    public interface IProdutoRepository
    {
        Task<Produto> ObterAsync(Guid id, CancellationToken cancellationToken = default);
        void Registrar(Produto entity);
        void Atualizar(Produto entity);
        void Excluir(Produto entity);
    }
}
