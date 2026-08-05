using Barbearia.Domain.Entidades;
using Barbearia.Infrastructure.Repositorios;
using Barbearia.Tests.Infrastructure.BancoDeDados;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Tests.Infrastructure.Repositorios;

[Collection(PostgreSqlCollection.NomeColecao)]
public sealed class ClienteRepositorioTests
{
    private readonly PostgreSqlFixture _fixture;

    public ClienteRepositorioTests(
        PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AdicionarAsync_DevePersistirCliente()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        _fixture.DbContext.ChangeTracker.Clear();

        var persistido = await _fixture.DbContext.Clientes
            .AsNoTracking()
            .SingleAsync();

        Assert.Equal(cliente.Id, persistido.Id);
        Assert.Equal("Daniel Luiz", persistido.Nome);
        Assert.Equal("11999999999", persistido.Telefone);
        Assert.Equal("daniel@email.com", persistido.Email);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarCliente_QuandoExistir()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "Maria Silva",
            "11888888888",
            "maria@email.com");

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        _fixture.DbContext.ChangeTracker.Clear();

        var resultado = await repositorio.ObterPorIdAsync(
            cliente.Id,
            CancellationToken.None);

        Assert.NotNull(resultado);
        Assert.Equal(cliente.Id, resultado.Id);
        Assert.Equal("Maria Silva", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var resultado = await repositorio.ObterPorIdAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarClientesOrdenadosPorNome()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var clienteZ = new Cliente(
            "Zeca Silva",
            "11777777777",
            null);

        var clienteA = new Cliente(
            "Ana Souza",
            "11666666666",
            null);

        await repositorio.AdicionarAsync(
            clienteZ,
            CancellationToken.None);

        await repositorio.AdicionarAsync(
            clienteA,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        var resultado = await repositorio.ListarAsync(
            CancellationToken.None);

        Assert.Equal(2, resultado.Count);
        Assert.Equal("Ana Souza", resultado.First().Nome);
        Assert.Equal("Zeca Silva", resultado.Last().Nome);
    }

    [Fact]
    public async Task ExisteTelefoneAsync_DeveRetornarTrue_QuandoTelefoneExistir()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "Carlos Lima",
            "11555555555",
            null);

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        var resultado = await repositorio.ExisteTelefoneAsync(
            "11555555555",
            CancellationToken.None);

        Assert.True(resultado);
    }

    [Fact]
    public async Task ExisteTelefoneAsync_DeveRetornarFalse_QuandoTelefoneNaoExistir()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var resultado = await repositorio.ExisteTelefoneAsync(
            "11444444444",
            CancellationToken.None);

        Assert.False(resultado);
    }

    [Fact]
    public async Task ExisteTelefoneParaOutroClienteAsync_DeveIgnorarClienteAtual()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "Pedro Alves",
            "11333333333",
            null);

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        var resultado =
            await repositorio.ExisteTelefoneParaOutroClienteAsync(
                cliente.Telefone,
                cliente.Id,
                CancellationToken.None);

        Assert.False(resultado);
    }

    [Fact]
    public async Task ExisteTelefoneParaOutroClienteAsync_DeveRetornarTrue_QuandoPertencerAOutroCliente()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "João Costa",
            "11222222222",
            null);

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        var resultado =
            await repositorio.ExisteTelefoneParaOutroClienteAsync(
                cliente.Telefone,
                Guid.NewGuid(),
                CancellationToken.None);

        Assert.True(resultado);
    }

    [Fact]
    public async Task SalvarAlteracoesAsync_DevePersistirAtualizacaoDoCliente()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var cliente = new Cliente(
            "Cliente Antigo",
            "11111111111",
            "antigo@email.com");

        await repositorio.AdicionarAsync(
            cliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        cliente.Atualizar(
            "Cliente Atualizado",
            "11000000000",
            "atualizado@email.com");

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        _fixture.DbContext.ChangeTracker.Clear();

        var atualizado = await _fixture.DbContext.Clientes
            .AsNoTracking()
            .SingleAsync();

        Assert.Equal("Cliente Atualizado", atualizado.Nome);
        Assert.Equal("11000000000", atualizado.Telefone);
        Assert.Equal(
            "atualizado@email.com",
            atualizado.Email);
    }

    [Fact]
    public async Task Banco_DeveImpedirDoisClientesComMesmoTelefone()
    {
        await _fixture.LimparBancoAsync();

        var repositorio =
            new ClienteRepositorio(_fixture.DbContext);

        var primeiroCliente = new Cliente(
            "Primeiro Cliente",
            "11912345678",
            null);

        var segundoCliente = new Cliente(
            "Segundo Cliente",
            "11912345678",
            null);

        await repositorio.AdicionarAsync(
            primeiroCliente,
            CancellationToken.None);

        await repositorio.SalvarAlteracoesAsync(
            CancellationToken.None);

        await repositorio.AdicionarAsync(
            segundoCliente,
            CancellationToken.None);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => repositorio.SalvarAlteracoesAsync(
                CancellationToken.None));

        _fixture.DbContext.ChangeTracker.Clear();
    }
}