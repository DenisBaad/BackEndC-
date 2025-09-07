using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Clientes.Delete;
public class DeleteClienteUseCase : IDeleteClienteUseCase
{
    private readonly IClienteWriteOnlyRepository _clienteWriteOnlyRepository;
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteClienteUseCase> _logger;

    public DeleteClienteUseCase(
        IClienteWriteOnlyRepository clienteWriteOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteClienteUseCase> logger)
    {
        _clienteWriteOnlyRepository = clienteWriteOnlyRepository;
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(Guid id)
    {
        try
        {
            var cliente = await _clienteReadOnlyRepository.GetById(id);
            _clienteWriteOnlyRepository.Delete(cliente);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar cliente com id: {id}", id);
            throw;
        }
    }
}
