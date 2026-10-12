using Barbearia.Infrastructure.Dados.Contextos;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Tests.Infrastructure.BancoDeDados;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    public BarbeariaDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var connectionString =
            ConfiguracaoTestes.ObterConnectionString();

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