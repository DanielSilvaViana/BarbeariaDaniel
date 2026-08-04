using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Application.DTOs.Clientes;

namespace Barbearia.Application.Interfaces.CasosDeUso.Clientes;

public interface IListarClientesCasoDeUso
{
    Task<IReadOnlyCollection<ClienteResponse>> ExecutarAsync(
        CancellationToken cancellationToken = default);
}
