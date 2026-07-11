using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Application.DTOs.Clientes;

namespace Barbearia.Application.Interfaces.CasosDeUso.Clientes;

public interface ICadastrarClienteCasoDeUso
{
    Task<ClienteResponse> ExecutarAsync(
        CadastrarClienteRequest request,
        CancellationToken cancellationToken);
}
