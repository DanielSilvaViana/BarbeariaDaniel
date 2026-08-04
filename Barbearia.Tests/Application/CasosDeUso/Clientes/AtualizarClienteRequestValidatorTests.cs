using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Validacoes.Clientes;

namespace Barbearia.Tests.Application.Validacoes.Clientes;

public sealed class AtualizarClienteRequestValidatorTests
{
    private readonly AtualizarClienteRequestValidator _validator = new();

    [Fact]
    public async Task ValidarAsync_DeveSerValido_QuandoDadosForemCorretos()
    {
        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            "daniel.silva@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
        Assert.Empty(resultado.Errors);
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoNomeEstiverVazio()
    {
        var request = new AtualizarClienteRequest(
            string.Empty,
            "11888888888",
            "daniel.silva@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Nome));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoNomeExcederTamanhoMaximo()
    {
        var request = new AtualizarClienteRequest(
            new string('A', 151),
            "11888888888",
            "daniel.silva@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Nome));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoTelefoneEstiverVazio()
    {
        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            string.Empty,
            "daniel.silva@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Telefone));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoTelefoneExcederTamanhoMaximo()
    {
        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            new string('1', 21),
            "daniel.silva@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Telefone));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoEmailForInvalido()
    {
        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            "email-invalido");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Email));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerValido_QuandoEmailNaoForInformado()
    {
        var request = new AtualizarClienteRequest(
            "Daniel Silva",
            "11888888888",
            null);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }
}