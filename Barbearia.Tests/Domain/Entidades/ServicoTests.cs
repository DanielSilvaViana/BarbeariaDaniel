using System;
using System.Collections.Generic;
using System.Text;
using Barbearia.Domain.Entidades;

namespace Barbearia.Tests.Domain.Entidades;

public class ServicoTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarServicoAtivo()
    {
        var servico = new Servico(
            "Corte masculino",
            50m,
            30,
            "Corte tradicional");

        Assert.NotEqual(Guid.Empty, servico.Id);
        Assert.Equal("Corte masculino", servico.Nome);
        Assert.Equal("Corte tradicional", servico.Descricao);
        Assert.Equal(50m, servico.Preco);
        Assert.Equal(30, servico.DuracaoMinutos);
        Assert.True(servico.Ativo);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Criar_ComNomeInvalido_DeveLancarExcecao(
        string nome)
    {
        var acao = () => new Servico(
            nome,
            50m,
            30);

        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "O nome do serviço é obrigatório.",
            excecao.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Criar_ComPrecoInvalido_DeveLancarExcecao(
        int preco)
    {
        var acao = () => new Servico(
            "Corte",
            preco,
            30);

        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "O preço deve ser maior que zero.",
            excecao.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-30)]
    public void Criar_ComDuracaoInvalida_DeveLancarExcecao(
        int duracao)
    {
        var acao = () => new Servico(
            "Corte",
            50m,
            duracao);

        var excecao = Assert.Throws<ArgumentException>(acao);

        Assert.Equal(
            "A duração deve ser maior que zero.",
            excecao.Message);
    }

    [Fact]
    public void Desativar_QuandoAtivo_DeveFicarInativo()
    {
        var servico = new Servico(
            "Corte",
            50m,
            30);

        servico.Desativar();

        Assert.False(servico.Ativo);
    }

    [Fact]
    public void Ativar_QuandoInativo_DeveFicarAtivo()
    {
        var servico = new Servico(
            "Corte",
            50m,
            30);

        servico.Desativar();
        servico.Ativar();

        Assert.True(servico.Ativo);
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAlterarServico()
    {
        var servico = new Servico(
            "Corte",
            50m,
            30);

        servico.Atualizar(
            "Corte premium",
            80m,
            45,
            "Corte com acabamento");

        Assert.Equal("Corte premium", servico.Nome);
        Assert.Equal("Corte com acabamento", servico.Descricao);
        Assert.Equal(80m, servico.Preco);
        Assert.Equal(45, servico.DuracaoMinutos);
    }
}
