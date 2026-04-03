using Aquiles.Communication.Enums;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories.Faturas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aquiles.Infrastructure.BackgroundServices.Faturas
{
    public class FaturaBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FaturaBackgroundService> _logger;

        public FaturaBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<FaturaBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando o serviço de verificação de faturas vencidas.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessarFaturasVencidas();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task ProcessarFaturasVencidas()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var readOnlyRepository = scope.ServiceProvider.GetRequiredService<IFaturaReadOnlyRepository>();
                var clienteUpdateOnlyRepository = scope.ServiceProvider.GetRequiredService<IClienteUpdateOnlyRepository>();
                var faturaUpdateOnlyRepository = scope.ServiceProvider.GetRequiredService<IFaturaUpdateOnlyRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var hoje = DateTime.UtcNow;

                var faturasPaged = await readOnlyRepository.GetAll(null, 1, 1000000);

                if (faturasPaged == null || faturasPaged.Items == null || !faturasPaged.Items.Any())
                {
                    _logger.LogInformation("Nenhuma fatura encontrada.");
                    return;
                }

                var faturasVencidas = faturasPaged.Items
                    .Where(f => f.DataVencimento < hoje && f.DataPagamento == null)
                    .ToList();

                if (!faturasVencidas.Any())
                {
                    _logger.LogInformation("Nenhuma fatura vencida encontrada.");
                    return;
                }

                foreach (var fatura in faturasVencidas)
                {
                    var cliente = await clienteUpdateOnlyRepository.GetById(fatura.ClienteId);

                    if (cliente != null)
                    {
                        if (cliente.Status != EnumStatusCliente.Inativo)
                        {
                            cliente.Status = EnumStatusCliente.Inativo;
                            clienteUpdateOnlyRepository.Update(cliente);

                            fatura.Status = EnumStatusFatura.Atrasado;
                            faturaUpdateOnlyRepository.Update(fatura);

                            await unitOfWork.CommitAsync();

                            _logger.LogInformation($"Cliente {cliente.Id} foi inativado e fatura {fatura.Id} foi marcada como atrasada.");
                        }
                    }
                }
            }
        }
    }
}
