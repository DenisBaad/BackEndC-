using Aquiles.Communication.Contracts;
using Confluent.Kafka;
using Enderecos.Application.UseCases.Enderecos.Create;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Enderecos.Infrastructure.Services;
public class CreateClienteConsumerService : BackgroundService
{
    private readonly ILogger<CreateClienteConsumerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public CreateClienteConsumerService(
        ILogger<CreateClienteConsumerService> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(async () =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka:BootstrapServers").Value,
                GroupId = _configuration.GetSection("Kafka:GroupId").Value,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("clientes-criados");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = consumer.Consume(stoppingToken);
                    var evento = JsonConvert.DeserializeObject<ClienteEvent>(cr.Message.Value);

                    if (evento != null)
                    {
                        _logger.LogInformation($"Novo cliente recebido: {evento.ClienteId}");

                        using var scope = _scopeFactory.CreateScope();
                        var createEnderecoUseCase = scope.ServiceProvider.GetRequiredService<ICreateEnderecoUseCase>();

                        evento.Endereco.ClienteId = evento.ClienteId;
                        await createEnderecoUseCase.Execute(evento.Endereco);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
