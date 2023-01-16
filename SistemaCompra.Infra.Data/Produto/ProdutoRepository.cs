using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;

namespace SistemaCompra.Infra.Data.Produto
{
    public class ProdutoRepository : ProdutoAgg.IProdutoRepository
    {
        private readonly SistemaCompraContext context;

        public ProdutoRepository(SistemaCompraContext context)  {
            this.context = context;
        }

        public void Atualizar(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Update(entity);
        }

        public void Excluir(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Remove(entity);
        }

        public Task<ProdutoAgg.Produto> ObterAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.Set<ProdutoAgg.Produto>().Where(c=> c.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public void Registrar(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Add(entity);
        }
    }
}
