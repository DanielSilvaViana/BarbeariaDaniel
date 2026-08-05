using System.Net;
using System.Net.Http.Json;
using Barbearia.Application.DTOs.Clientes;

namespace Barbearia.Tests.Api;

public sealed class ClientesEndpointTests
    : IClassFixture<BarbeariaApiFactory>
{
    private readonly HttpClient _client;

    public ClientesEndpointTests(
        BarbeariaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_DeveRetornar201_QuandoRequestForValido()
    {
        var request = new CadastrarClienteRequest(
            "Daniel Luiz",
            "11988887777",
            "daniel@email.com");

        var response = await _client.PostAsJsonAsync(
            "/api/clientes",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var cliente =
            await response.Content
                .ReadFromJsonAsync<ClienteResponse>();

        Assert.NotNull(cliente);
        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal(request.Nome, cliente.Nome);
        Assert.Equal(request.Telefone, cliente.Telefone);
    }

    [Fact]
    public async Task GetPorId_DeveRetornar404_QuandoClienteNaoExistir()
    {
        var response = await _client.GetAsync(
            $"/api/clientes/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Post_DeveRetornar400_QuandoRequestForInvalido()
    {
        var request = new CadastrarClienteRequest(
            string.Empty,
            "123",
            "email-invalido");

        var response = await _client.PostAsJsonAsync(
            "/api/clientes",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Post_DeveRetornar409_QuandoTelefoneJaExistir()
    {
        var primeiroRequest = new CadastrarClienteRequest(
            "Primeiro Cliente",
            "11911112222",
            null);

        var segundoRequest = new CadastrarClienteRequest(
            "Segundo Cliente",
            "11911112222",
            null);

        var primeiraResposta =
            await _client.PostAsJsonAsync(
                "/api/clientes",
                primeiroRequest);

        var segundaResposta =
            await _client.PostAsJsonAsync(
                "/api/clientes",
                segundoRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            primeiraResposta.StatusCode);

        Assert.Equal(
            HttpStatusCode.Conflict,
            segundaResposta.StatusCode);
    }
}