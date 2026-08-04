using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Application.Mapeamentos;

namespace Barbearia.Application.CasosDeUso.Clientes;

public sealed class ListarClientesCasoDeUso : IListarClientesCasoDeUso
{
    private readonly IClienteRepositorio _clienteRepositorio;

    public ListarClientesCasoDeUso(
        IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    public async Task<IReadOnlyCollection<ClienteResponse>> ExecutarAsync(
        CancellationToken cancellationToken = default)
    {
        var clientes = await _clienteRepositorio.ListarAsync(
            cancellationToken);

        return clientes
            .Select(cliente => cliente.ParaResponse())
            .ToArray();
    }
}