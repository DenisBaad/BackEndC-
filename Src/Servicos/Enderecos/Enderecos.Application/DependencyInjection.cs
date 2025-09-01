using Aquiles.Utils.Filters;
using Aquiles.Utils.Services;
using Aquiles.Utils.UsuarioLogado;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enderecos.Application;
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddRepositories(services);
        AdicionarTokenJWT(services, configurationManager);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        AdicionarUsuarioLogadoUseCase(services);
    }

    private static void AdicionarUsuarioLogadoUseCase(IServiceCollection services)
    {
        services.AddScoped<IUsuarioLogado, UsuarioLogado>();
    }

    private static void AdicionarTokenJWT(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(option =>
            new TokenController(
                configuration.GetSection("Configuracoes:Jwt:TokenKey").Value,
                int.Parse(configuration.GetSection("Configuracoes:Jwt:LifeTimeMinutes").Value)));

        services.AddScoped(option => new AquilesAuthorize(new TokenController(
                configuration.GetSection("Configuracoes:Jwt:TokenKey").Value,
                int.Parse(configuration.GetSection("Configuracoes:Jwt:LifeTimeMinutes").Value))));
    }
}
