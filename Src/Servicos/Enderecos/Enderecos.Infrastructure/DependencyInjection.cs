using Aquiles.Utils.Extensions;
using Enderecos.Domain.Repositories;
using Enderecos.Infrastructure.Context;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Enderecos.Infrastructure;
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
    }

    private static void AddUnitOfWorkRepository(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddMySqlContext(IServiceCollection services, IConfiguration configuration)
    {
        var versaoServidor = new MySqlServerVersion(new Version(5, 6));

        var connectionString = configuration.GetConexaoCompleta();

        services.AddDbContext<EnderecosContext>(context => context.UseMySql(connectionString, versaoServidor));
    }

    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConexaoCompleta();

        services.AddFluentMigratorCore().ConfigureRunner(options => options.AddMySql5().WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("Enderecos.Infrastructure")).For.All());
    }
}
