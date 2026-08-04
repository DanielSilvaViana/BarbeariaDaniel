using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Application.DTOs.Clientes;

namespace Barbearia.Application.Interfaces.CasosDeUso.Clientes;

public interface IAtualizarClienteCasoDeUso
{
    Task<ClienteResponse> ExecutarAsync(
        Guid id,
        AtualizarClienteRequest request,
        CancellationToken cancellationToken = default);
}