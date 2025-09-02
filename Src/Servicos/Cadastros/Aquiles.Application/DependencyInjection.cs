using Aquiles.Application.UseCases.Clientes.AtivarOuInativar;
using Aquiles.Application.UseCases.Clientes.Create;
using Aquiles.Application.UseCases.Clientes.Delete;
using Aquiles.Application.UseCases.Clientes.GetAll;
using Aquiles.Application.UseCases.Clientes.GetById;
using Aquiles.Application.UseCases.Clientes.Update;
using Aquiles.Application.UseCases.Faturas.Create;
using Aquiles.Application.UseCases.Faturas.GetAll;
using Aquiles.Application.UseCases.Faturas.Update;
using Aquiles.Application.UseCases.Login.DoLogin;
using Aquiles.Application.UseCases.Planos.Create;
using Aquiles.Application.UseCases.Planos.GetAll;
using Aquiles.Application.UseCases.Planos.GetById;
using Aquiles.Application.UseCases.Planos.Update;
using Aquiles.Application.UseCases.Relatorios.RelatorioFaturas;
using Aquiles.Application.UseCases.Usuarios.Create;
using Aquiles.Utils.Filters;
using Aquiles.Utils.Services;
using Aquiles.Utils.UsuarioLogado;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aquiles.Application;
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddRepositories(services);
        AdicionarKafka(services, configurationManager);
        AdicionarChaveAdicionalToken(services, configurationManager);
        AdicionarTokenJWT(services, configurationManager);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        AddUsuarioUseCase(services);
        AddLoginUseCase(services);
        AdicionarUsuarioLogadoUseCase(services);
        AddClienteUseCase(services);
        AddPlanoUseCase(services);
        AddFaturaUseCase(services);
    }

    private static void AddUsuarioUseCase(IServiceCollection services)
    {
        services.AddScoped<ICreateUsuarioUseCase, CreateUsuarioUseCase>();
    }

    private static void AddLoginUseCase(IServiceCollection services)
    {
        services.AddScoped<ILoginUseCase, LoginUseCase>();
    }

    private static void AdicionarUsuarioLogadoUseCase(IServiceCollection services)
    {
        services.AddScoped<IUsuarioLogado, UsuarioLogado>();
    }

    private static void AddClienteUseCase(IServiceCollection services) 
    {
        services
            .AddScoped<ICreateClienteUseCase, CreateClienteUseCase>()
            .AddScoped<IGetAllClientesUseCase, GetAllClientesUseCase>()
            .AddScoped<IGetIdClientesUseCase,  GetIdClientesUseCase>()
            .AddScoped<IUpdateClienteUseCase,  UpdateClienteUseCase>()
            .AddScoped<IDeleteClienteUseCase, DeleteClienteUseCase>()
            .AddScoped<IAtivarOuInativarClienteUseCase, AtivarOuInativarClienteUseCase>();
    }

    private static void AddPlanoUseCase(IServiceCollection services)
    {
        services
            .AddScoped<ICreatePlanoUseCase, CreatePlanoUseCase>()
            .AddScoped<IGetAllPlanoUseCase, GetAllPlanoUseCase>()
            .AddScoped<IGetIdPlanoUseCase, GetIdPlanoUseCase>()
            .AddScoped<IUpdatePlanoUseCase, UpdatePlanoUseCase>();
    }

    private static void AddFaturaUseCase(IServiceCollection services)
    {
        services
            .AddScoped<ICreateFaturaUseCase, CreateFaturaUseCase>()
            .AddScoped<IGetAllFaturaUseCase, GetAllFaturaUseCase>()
            .AddScoped<IUpdateFaturaUseCase, UpdateFaturaUseCase>()
            .AddScoped<IRelatorioFaturas, RelatorioFaturas>();
    }
    
    private static void AdicionarChaveAdicionalToken(IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddScoped(option => new PasswordEncrypt(configurationManager.GetSection("Configuracoes:Senha:ChaveAdicionalSenha").Value));
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

    private static void AdicionarKafka(IServiceCollection services, IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration.GetSection("Kafka:BootstrapServers").Value
        };

        services.AddSingleton<IProducer<Null, string>>(sp => new ProducerBuilder<Null, string>(producerConfig).Build());
    }
}
