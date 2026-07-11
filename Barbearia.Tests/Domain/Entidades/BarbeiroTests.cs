using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Domain.Entidades;

namespace Barbearia.Tests.Domain.Entidades;

public class BarbeiroTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarBarbeiroAtivo()
    {
        var barbeiro = new Barbeiro(
            "João",
            "Corte degradê");

        Assert.NotEqual(Guid.Empty, barbeiro.Id);
        Assert.Equal("João", barbeiro.Nome);
        Assert.Equal("Corte degradê", barbeiro.Especialidade);
        Assert.True(barbeiro.Ativo);
        Assert.True(barbeiro.CriadoEm <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Criar_ComNomeInvalido_DeveLancarExcecao(
        string nome)
    {
        var acao = () => new Barbeiro(nome);

        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "O nome do barbeiro é obrigatório.",
            excecao.Message);
    }

    [Fact]
    public void Criar_ComEspacos_DeveRemoverEspacosExternos()
    {
        var barbeiro = new Barbeiro(
            "  João  ",
            "  Corte e barba  ");

        Assert.Equal("João", barbeiro.Nome);
        Assert.Equal("Corte e barba", barbeiro.Especialidade);
    }

    [Fact]
    public void Desativar_QuandoAtivo_DeveFicarInativo()
    {
        var barbeiro = new Barbeiro("João");

        barbeiro.Desativar();

        Assert.False(barbeiro.Ativo);
    }

    [Fact]
    public void Ativar_QuandoInativo_DeveFicarAtivo()
    {
        var barbeiro = new Barbeiro("João");
        barbeiro.Desativar();

        barbeiro.Ativar();

        Assert.True(barbeiro.Ativo);
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAlterarDados()
    {
        var barbeiro = new Barbeiro(
            "João",
            "Barba");

        barbeiro.Atualizar(
            "João Silva",
            "Corte e barba");

        Assert.Equal("João Silva", barbeiro.Nome);
        Assert.Equal("Corte e barba", barbeiro.Especialidade);
    }
}