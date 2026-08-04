using Barbearia.Application.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Barbearia.Application.Dependencias;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(InjecaoDependencia).Assembly);

        services.AddScoped<
            ICadastrarClienteCasoDeUso,
            CadastrarClienteCasoDeUso>();

        services.AddScoped<
            IConsultarClienteCasoDeUso,
            ConsultarClienteCasoDeUso>();

        services.AddScoped<
            IListarClientesCasoDeUso,
            ListarClientesCasoDeUso>();

        services.AddScoped<
            IAtualizarClienteCasoDeUso,
            AtualizarClienteCasoDeUso>();

        return services;
    }
}