using Aquiles.Communication.Enums;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Clientes.AtivarOuInativar;
public class AtivarOuInativarClienteUseCase : IAtivarOuInativarClienteUseCase
{
    private readonly IClienteUpdateOnlyRepository _clienteUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private ILogger<AtivarOuInativarClienteUseCase> _logger;

    public AtivarOuInativarClienteUseCase(
        IClienteUpdateOnlyRepository clienteUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        ILogger<AtivarOuInativarClienteUseCase> logger)
    {
        _clienteUpdateOnlyRepository = clienteUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(Guid id)
    {
        try
        {
            var cliente = await _clienteUpdateOnlyRepository.GetById(id);

            if (cliente.Status == EnumStatusCliente.Ativo)
            {
                cliente.Status = EnumStatusCliente.Inativo;
            }
            else
            {
                cliente.Status = EnumStatusCliente.Ativo;
            }

            _clienteUpdateOnlyRepository.Update(cliente);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar/inatiuvar cliente com id: {ClienteId}", id);
            throw;
        }
    }
}
