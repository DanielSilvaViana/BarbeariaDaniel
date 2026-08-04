using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Validacoes.Clientes;

namespace Barbearia.Tests.Application.Validacoes.Clientes;

public sealed class CadastrarClienteRequestValidatorTests
{
    private readonly CadastrarClienteRequestValidator _validator = new();

    [Fact]
    public async Task ValidarAsync_DeveSerValido_QuandoDadosForemCorretos()
    {
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11999999999",
            "daniel@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
        Assert.Empty(resultado.Errors);
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoNomeEstiverVazio()
    {
        var request = new CadastrarClienteRequest(
            string.Empty,
            "11999999999",
            "daniel@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Nome));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoNomeTiverMenosDeTresCaracteres()
    {
        var request = new CadastrarClienteRequest(
            "Da",
            "11999999999",
            "daniel@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Nome));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoTelefoneEstiverVazio()
    {
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            string.Empty,
            "daniel@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Telefone));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoTelefoneForMuitoCurto()
    {
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "1234567",
            "daniel@email.com");

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(request.Telefone));
    }

    [Fact]
    public async Task ValidarAsync_DeveSerInvalido_QuandoEmailForInvalido()
    {
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11999999999",
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
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11999999999",
            null);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }
}