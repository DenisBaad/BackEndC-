using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Domain.Repositories.Usuarios;
using Aquiles.Infrastructure.Context;
using Aquiles.Infrastructure.Repositories;
using Aquiles.Utils.Extensions;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aquiles.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);

        if (configuration.IsUnitTestEnvironment())
            return;
        
        AddMySqlContext(services, configuration);
        AddFluentMigrator_MySql(services, configuration);
    }
    
    public static void AddRepositories(IServiceCollection services)
    {
        AddUnitOfWorkRepository(services);
        AddUsuariosRepository(services);
        AddClientesRepository(services);
        AddPlanoRepository(services);
        AddFaturaRepository(services);
    }

    private static void AddUnitOfWorkRepository(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddUsuariosRepository(IServiceCollection services) 
    {
        services
            .AddScoped<IUsuarioWriteOnlyRepository, UsuarioRepository>()
            .AddScoped<IUsuarioReadOnlyRepository, UsuarioRepository>();
    }

    public static void AddClientesRepository(IServiceCollection services)
    {
        services
            .AddScoped<IClienteWriteOnlyRepository, ClienteRepository>()
            .AddScoped<IClienteReadOnlyRepository, ClienteRepository>()
            .AddScoped<IClienteUpdateOnlyRepository, ClienteRepository>();
    }

    private static void AddPlanoRepository(IServiceCollection services)
    {
        services
            .AddScoped<IPlanoWriteOnlyRepository, PlanoRepository>()
            .AddScoped<IPlanoReadOnlyRepository, PlanoRepository>()
            .AddScoped<IPlanoUpdateOnlyRepository, PlanoRepository>();
    }

    private static void AddFaturaRepository(IServiceCollection services)
    {
        services
            .AddScoped<IFaturaWriteOnlyRepository, FaturaRepository>()
            .AddScoped<IFaturaReadOnlyRepository, FaturaRepository>()
            .AddScoped<IFaturaUpdateOnlyRepository, FaturaRepository>();
    }

    public static void AddMySqlContext(IServiceCollection services, IConfiguration configuration)
    {
        var versaoServidor = new MySqlServerVersion(new Version(5, 6));

        var connectionString = configuration.GetConexaoCompleta();

        services.AddDbContext<AquilesContext>(context => context.UseMySql(connectionString, versaoServidor));
    }

    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConexaoCompleta();

        services.AddFluentMigratorCore().ConfigureRunner(options => options.AddMySql5().WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("Aquiles.Infrastructure")).For.All());
    }
}
