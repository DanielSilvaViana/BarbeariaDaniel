using Barbearia.Application.DTOs.Clientes;
using Barbearia.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Barbearia.Application.Mapeamentos
{
    public static class ClienteMapeamento
    {
        public static ClienteResponse ParaResponse(this Cliente cliente)
        {
            return new ClienteResponse(
                cliente.Id,
                cliente.Nome,
                cliente.Telefone,
                cliente.Email,
                cliente.CriadoEm);
        }

    }
}
