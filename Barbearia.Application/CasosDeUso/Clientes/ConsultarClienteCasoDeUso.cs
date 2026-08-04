using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Application.Mapeamentos;

namespace Barbearia.Application.CasosDeUso.Clientes;

public sealed class ConsultarClienteCasoDeUso : IConsultarClienteCasoDeUso
{
    private readonly IClienteRepositorio _clienteRepositorio;

    public ConsultarClienteCasoDeUso(
        IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    public async Task<ClienteResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepositorio.ObterPorIdAsync(
            id,
            cancellationToken);

        if (cliente is null)
        {
            throw new NaoEncontradoException(
                "Cliente não encontrado.");
        }

        return cliente.ParaResponse();
    }
}