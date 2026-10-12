using Barbearia.Tests.Infrastructure.BancoDeDados;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Barbearia.Tests.Api;

public sealed class BarbeariaApiFactory
    : WebApplicationFactory<global::Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // UseSetting é aplicado antes de o Program.cs ler a configuração.
        // Migrations e limpeza do banco ficam a cargo do PostgreSqlFixture.
        builder.UseSetting(
            "ConnectionStrings:PostgreSql",
            ConfiguracaoTestes.ObterConnectionString());
    }
}
