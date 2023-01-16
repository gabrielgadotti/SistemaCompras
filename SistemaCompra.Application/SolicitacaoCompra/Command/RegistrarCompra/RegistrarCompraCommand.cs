﻿using MediatR;
using System;
using System.Collections.Generic;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<bool>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public IEnumerable<ItemCompra> Itens { get; set; }
        public int CondicaoPagamento { get; set; }

        public class ItemCompra
        {
            public Guid ProdutoId { get; set; }
            public int Quantidade { get; set; }
        }
    }
}
