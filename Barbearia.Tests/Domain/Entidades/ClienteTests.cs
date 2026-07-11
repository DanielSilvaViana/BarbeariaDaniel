using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Domain.Entidades;

namespace Barbearia.Tests.Domain.Entidades;

public class ClienteTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarCliente()
    {
        // Arrange
        const string nome = "Daniel Silva";
        const string telefone = "11999999999";
        const string email = "daniel@email.com";

        // Act
        var cliente = new Cliente(nome, telefone, email);

        // Assert
        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal(nome, cliente.Nome);
        Assert.Equal(telefone, cliente.Telefone);
        Assert.Equal(email, cliente.Email);
        Assert.True(cliente.CriadoEm <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Criar_ComNomeInvalido_DeveLancarExcecao(string nome)
    {
        // Act
        var acao = () => new Cliente(
            nome,
            "11999999999");

        // Assert
        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "O nome do cliente é obrigatório.",
            excecao.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Criar_ComTelefoneInvalido_DeveLancarExcecao(
        string telefone)
    {
        var acao = () => new Cliente(
            "Daniel Silva",
            telefone);

        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "O telefone do cliente é obrigatório.",
            excecao.Message);
    }

    [Fact]
    public void Criar_ComEspacosExternos_DeveRemoverEspacos()
    {
        var cliente = new Cliente(
            "  Daniel Silva  ",
            "  11999999999  ",
            "  daniel@email.com  ");

        Assert.Equal("Daniel Silva", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("daniel@email.com", cliente.Email);
    }

    [Fact]
    public void Criar_SemEmail_DevePermitirEmailNulo()
    {
        var cliente = new Cliente(
            "Daniel Silva",
            "11999999999");

        Assert.Null(cliente.Email);
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAlterarCliente()
    {
        var cliente = new Cliente(
            "Daniel",
            "11111111111");

        cliente.Atualizar(
            "Daniel Silva",
            "11999999999",
            "daniel@email.com");

        Assert.Equal("Daniel Silva", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("daniel@email.com", cliente.Email);
    }

    [Fact]
    public void Atualizar_ComNomeInvalido_DeveManterDadosAnteriores()
    {
        var cliente = new Cliente(
            "Daniel Silva",
            "11999999999",
            "daniel@email.com");

        var acao = () => cliente.Atualizar(
            "",
            "11888888888",
            "novo@email.com");

        Assert.Throws<ArgumentException>(acao);

        Assert.Equal("Daniel Silva", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("daniel@email.com", cliente.Email);
    }
}
