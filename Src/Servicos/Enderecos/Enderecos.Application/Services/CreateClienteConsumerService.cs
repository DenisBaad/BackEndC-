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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = _configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("clientes-criados");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var evento = JsonConvert.DeserializeObject<ClienteEvent>(consumeResult.Message.Value);

                    _logger.LogInformation("Novo cliente recebido: {ClienteId}", evento.ClienteId);

                    evento.Endereco.ClienteId = evento.ClienteId;
                    using var scope = _scopeFactory.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<ICreateEnderecoUseCase>().Execute(evento.Endereco);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError("Erro ao consumir mensagem: {Reason}", e.Error.Reason);
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}