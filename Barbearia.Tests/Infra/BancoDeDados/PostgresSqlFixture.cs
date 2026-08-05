using Barbearia.Infrastructure.Dados.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Barbearia.Tests.Infrastructure.BancoDeDados;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    public BarbeariaDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.Test.json",
                optional: false)
            .Build();

        var connectionString =
            configuration.GetConnectionString("PostgreSql");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string de testes não foi configurada.");
        }

        var options =
            new DbContextOptionsBuilder<BarbeariaDbContext>()
                .UseNpgsql(connectionString)
                .Options;

        DbContext = new BarbeariaDbContext(options);

        await DbContext.Database.MigrateAsync();

        await LimparBancoAsync();
    }

    public async Task LimparBancoAsync()
    {
        DbContext.ChangeTracker.Clear();

        await DbContext.Clientes.ExecuteDeleteAsync();
    }

    public async Task DisposeAsync()
    {
        await LimparBancoAsync();
        await DbContext.DisposeAsync();
    }
}