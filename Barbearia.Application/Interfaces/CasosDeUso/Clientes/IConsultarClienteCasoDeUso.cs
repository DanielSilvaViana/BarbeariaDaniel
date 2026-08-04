using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Application.DTOs.Clientes;

namespace Barbearia.Application.Interfaces.CasosDeUso.Clientes;

public interface IConsultarClienteCasoDeUso
{
    Task<ClienteResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}