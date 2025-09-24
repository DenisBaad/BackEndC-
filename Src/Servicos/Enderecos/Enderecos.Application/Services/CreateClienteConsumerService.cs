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
        while (!stoppingToken.IsCancellationRequested)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka:BootstrapServers").Value,
                GroupId = _configuration.GetSection("Kafka:GroupId").Value,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("clientes-criados");

                using var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);
                            var evento = JsonConvert.DeserializeObject<ClienteEvent>(consumeResult.Message.Value);

                            _logger.LogInformation($"Novo cliente recebido: {evento.ClienteId}");

                            evento.Endereco.ClienteId = evento.ClienteId;
                            await _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICreateEnderecoUseCase>().Execute(evento.Endereco);
                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogInformation($"Erro ao consumir mensagem: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}