using Barbearia.Application.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Domain.Entidades;
using Moq;

namespace Barbearia.Tests.Application.CasosDeUso.Clientes;

public sealed class ListarClientesCasoDeUsoTests
{
    [Fact]
    public async Task ExecutarAsync_DeveRetornarTodosOsClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new(
                "Daniel Luiz",
                "11999999999",
                "daniel@email.com"),

            new(
                "Maria Silva",
                "11888888888",
                "maria@email.com")
        };

        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ListarAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        var casoDeUso = new ListarClientesCasoDeUso(
            repositorioMock.Object);

        // Act
        var response = await casoDeUso.ExecutarAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Count);

        Assert.Contains(
            response,
            cliente => cliente.Nome == "Daniel Luiz");

        Assert.Contains(
            response,
            cliente => cliente.Nome == "Maria Silva");
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarColecaoVazia_QuandoNaoExistiremClientes()
    {
        // Arrange
        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ListarAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Cliente>());

        var casoDeUso = new ListarClientesCasoDeUso(
            repositorioMock.Object);

        // Act
        var response = await casoDeUso.ExecutarAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}