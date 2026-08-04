using Barbearia.Application.CasosDeUso.Clientes;
using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Domain.Entidades;
using Moq;

namespace Barbearia.Tests.Application.CasosDeUso.Clientes;

public sealed class AtualizarClienteCasoDeUsoTests
{
    [Fact]
    public async Task ExecutarAsync_DeveAtualizarCliente_QuandoDadosForemValidos()
    {
        // Arrange
        var cliente = new Cliente(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ObterPorIdAsync(
                cliente.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        repositorioMock
            .Setup(repositorio =>
                repositorio.ExisteTelefoneParaOutroClienteAsync(
                    It.IsAny<string>(),
                    cliente.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var casoDeUso = new AtualizarClienteCasoDeUso(
            repositorioMock.Object);

        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            "daniel.silva@email.com");

        // Act
        var response = await casoDeUso.ExecutarAsync(
            cliente.Id,
            request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(cliente.Id, response.Id);
        Assert.Equal("Daniel Silva", response.Nome);
        Assert.Equal("11888888888", response.Telefone);
        Assert.Equal("daniel.silva@email.com", response.Email);

        repositorioMock.Verify(
            repositorio => repositorio.SalvarAlteracoesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveLancarNaoEncontradoException_QuandoClienteNaoExistir()
    {
        // Arrange
        var clienteId = Guid.NewGuid();

        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ObterPorIdAsync(
                clienteId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        var casoDeUso = new AtualizarClienteCasoDeUso(
            repositorioMock.Object);

        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            "daniel.silva@email.com");

        // Act
        var excecao = await Assert.ThrowsAsync<NaoEncontradoException>(
            () => casoDeUso.ExecutarAsync(
                clienteId,
                request));

        // Assert
        Assert.Equal(
            "Cliente não encontrado.",
            excecao.Message);

        repositorioMock.Verify(
            repositorio =>
                repositorio.ExisteTelefoneParaOutroClienteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);

        repositorioMock.Verify(
            repositorio => repositorio.SalvarAlteracoesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_DeveLancarConflitoException_QuandoTelefonePertencerAOutroCliente()
    {
        // Arrange
        var cliente = new Cliente(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ObterPorIdAsync(
                cliente.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        repositorioMock
            .Setup(repositorio =>
                repositorio.ExisteTelefoneParaOutroClienteAsync(
                    "11888888888",
                    cliente.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var casoDeUso = new AtualizarClienteCasoDeUso(
            repositorioMock.Object);

        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            "daniel.silva@email.com");

        // Act
        var excecao = await Assert.ThrowsAsync<ConflitoException>(
            () => casoDeUso.ExecutarAsync(
                cliente.Id,
                request));

        // Assert
        Assert.Equal(
            "Já existe outro cliente cadastrado com este telefone.",
            excecao.Message);

        Assert.Equal("Daniel Luiz", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("daniel@email.com", cliente.Email);

        repositorioMock.Verify(
            repositorio => repositorio.SalvarAlteracoesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}