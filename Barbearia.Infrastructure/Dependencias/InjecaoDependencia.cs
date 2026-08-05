using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Infrastructure.Dados.Contextos;
using Barbearia.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barbearia.Infrastructure.Dependencias;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("PostgreSql");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string 'PostgreSql' não foi configurada.");
        }

        services.AddDbContext<BarbeariaDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString);
            });

        services.AddScoped<
            IClienteRepositorio,
            ClienteRepositorio>();

        return services;
    }
}