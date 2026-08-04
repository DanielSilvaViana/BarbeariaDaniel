using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Application.Mapeamentos;

namespace Barbearia.Application.CasosDeUso.Clientes;

public sealed class AtualizarClienteCasoDeUso : IAtualizarClienteCasoDeUso
{
    private readonly IClienteRepositorio _clienteRepositorio;

    public AtualizarClienteCasoDeUso(
        IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    public async Task<ClienteResponse> ExecutarAsync(
        Guid id,
        AtualizarClienteRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cliente = await _clienteRepositorio.ObterPorIdAsync(
            id,
            cancellationToken);

        if (cliente is null)
        {
            throw new NaoEncontradoException(
                "Cliente não encontrado.");
        }

        var telefonePertenceAOutroCliente =
            await _clienteRepositorio.ExisteTelefoneParaOutroClienteAsync(
                request.Telefone,
                id,
                cancellationToken);

        if (telefonePertenceAOutroCliente)
        {
            throw new ConflitoException(
                "Já existe outro cliente cadastrado com este telefone.");
        }

        cliente.Atualizar(
            request.Nome,
            request.Telefone,
            request.Email);

        await _clienteRepositorio.SalvarAlteracoesAsync(
            cancellationToken);

        return cliente.ParaResponse();
    }
}