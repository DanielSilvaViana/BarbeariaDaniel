using Barbearia.Infrastructure.Dados.Contextos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Barbearia.Tests.Api;

public sealed class BarbeariaApiFactory
    : WebApplicationFactory<global::Program>
{
    private readonly string _connectionString;

    public BarbeariaApiFactory()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.Test.json",
                optional: false)
            .Build();

        _connectionString =
            configuration.GetConnectionString("PostgreSql")
            ?? throw new InvalidOperationException(
                "A connection string de testes não foi configurada.");

        // Precisa existir antes de o Program.cs ser executado.
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__PostgreSql",
            _connectionString);
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(
            (_, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:PostgreSql"] =
                            _connectionString
                    });
            });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                DbContextOptions<BarbeariaDbContext>>();

            services.RemoveAll<BarbeariaDbContext>();

            services.AddDbContext<BarbeariaDbContext>(
                options =>
                    options.UseNpgsql(_connectionString));

            using var serviceProvider =
                services.BuildServiceProvider();

            using var scope =
                serviceProvider.CreateScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<BarbeariaDbContext>();

            dbContext.Database.Migrate();
            dbContext.Clientes.ExecuteDelete();
        });
    }

    protected override void Dispose(bool disposing)
    {
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__PostgreSql",
            null);

        base.Dispose(disposing);
    }
}