using Scalar.AspNetCore;
using Barbearia.Application.Dependencias;
using Barbearia.Infrastructure.Dependencias;
using BarbeariaApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AdicionarApplication();
builder.Services.AdicionarInfrastructure(
    builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Barbearia Daniel API")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(
                ScalarTarget.CSharp,
                ScalarClient.HttpClient);
    });
}

app.UseMiddleware<TratamentoExcecoesMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}