using Barbearia.Application.CasosDeUso.Clientes;
using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Domain.Entidades;
using Moq;

namespace Barbearia.Tests.Application.CasosDeUso.Clientes;

public sealed class CadastrarClienteCasoDeUsoTests
{
    [Fact]
    public async Task ExecutarAsync_DeveCadastrarCliente_QuandoTelefoneNaoExistir()
    {
        // Arrange
        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ExisteTelefoneAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var casoDeUso = new CadastrarClienteCasoDeUso(
            repositorioMock.Object);

        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        // Act
        var response = await casoDeUso.ExecutarAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Daniel Luiz", response.Nome);
        Assert.Equal("11999999999", response.Telefone);
        Assert.Equal("daniel@email.com", response.Email);

        repositorioMock.Verify(
            repositorio => repositorio.AdicionarAsync(
                It.IsAny<Cliente>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        repositorioMock.Verify(
            repositorio => repositorio.SalvarAlteracoesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveLancarConflitoException_QuandoTelefoneJaExistir()
    {
        // Arrange
        var repositorioMock = new Mock<IClienteRepositorio>();

        repositorioMock
            .Setup(repositorio => repositorio.ExisteTelefoneAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var casoDeUso = new CadastrarClienteCasoDeUso(
            repositorioMock.Object);

        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        // Act
        var excecao = await Assert.ThrowsAsync<ConflitoException>(
            () => casoDeUso.ExecutarAsync(request));

        // Assert
        Assert.Equal(
            "Já existe um cliente cadastrado com este telefone.",
            excecao.Message);

        repositorioMock.Verify(
            repositorio => repositorio.AdicionarAsync(
                It.IsAny<Cliente>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        repositorioMock.Verify(
            repositorio => repositorio.SalvarAlteracoesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}