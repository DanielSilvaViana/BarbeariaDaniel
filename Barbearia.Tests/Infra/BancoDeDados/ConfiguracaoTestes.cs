using Microsoft.Extensions.Configuration;

namespace Barbearia.Tests.Infrastructure.BancoDeDados;

public static class ConfiguracaoTestes
{
    // Local: appsettings.Test.json (copiado de appsettings.Test.example.json).
    // CI: variável de ambiente ConnectionStrings__PostgreSql.
    public static string ObterConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.Test.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("PostgreSql");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string de testes não foi configurada. " +
                "Copie Barbearia.Tests/appsettings.Test.example.json para " +
                "appsettings.Test.json ou defina a variável de ambiente " +
                "ConnectionStrings__PostgreSql.");
        }

        return connectionString;
    }
}
