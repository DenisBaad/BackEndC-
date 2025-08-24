using Aquiles.Domain.Entities;
using Aquiles.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AquilesContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<AquilesContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var scopeService = scope.ServiceProvider;
                var database = scopeService.GetRequiredService<AquilesContext>();

                database.Database.EnsureDeleted();

                // Criação das entidades no banco em memória para uso nos testes
                var contextInMemory = ContextSeedInMemory.Seed(database);
                _usuario = contextInMemory.usuario;
                _senha = contextInMemory.senha;
                _plano = contextInMemory.plano;
            });
    }

    // Variáveis privadas utilizadas para armazenar os dados criados no banco em memória
    private Usuario _usuario;
    private string _senha;
    private Plano _plano;


    // Métodos públicos de acesso aos dados criados para uso nos testes
    public Usuario GetUsuario()  => _usuario;
    public string GetSenha() => _senha;
    public Plano GetPlano() => _plano;
}
