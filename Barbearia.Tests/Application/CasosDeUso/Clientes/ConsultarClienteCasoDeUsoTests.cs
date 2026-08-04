using Barbearia.Application.CasosDeUso.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Domain.Entidades;
using Moq;

namespace Barbearia.Tests.Application.CasosDeUso.Clientes;

public sealed class ConsultarClienteCasoDeUsoTests
{
    [Fact]
    public async Task ExecutarAsync_DeveRetornarCliente_QuandoClienteExistir()
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

        var casoDeUso = new ConsultarClienteCasoDeUso(
            repositorioMock.Object);

        // Act
        var response = await casoDeUso.ExecutarAsync(cliente.Id);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(cliente.Id, response.Id);
        Assert.Equal(cliente.Nome, response.Nome);
        Assert.Equal(cliente.Telefone, response.Telefone);
        Assert.Equal(cliente.Email, response.Email);
        Assert.Equal(cliente.CriadoEm, response.CriadoEm);
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

        var casoDeUso = new ConsultarClienteCasoDeUso(
            repositorioMock.Object);

        // Act
        var excecao = await Assert.ThrowsAsync<NaoEncontradoException>(
            () => casoDeUso.ExecutarAsync(clienteId));

        // Assert
        Assert.Equal("Cliente não encontrado.", excecao.Message);
    }
}