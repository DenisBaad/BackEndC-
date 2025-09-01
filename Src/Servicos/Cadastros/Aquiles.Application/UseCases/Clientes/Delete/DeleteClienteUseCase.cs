using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;

namespace Aquiles.Application.UseCases.Clientes.Delete;
public class DeleteClienteUseCase : IDeleteClienteUseCase
{
    private readonly IClienteWriteOnlyRepository _clienteWriteOnlyRepository;
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClienteUseCase(
        IClienteWriteOnlyRepository clienteWriteOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _clienteWriteOnlyRepository = clienteWriteOnlyRepository;
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var cliente = await _clienteReadOnlyRepository.GetById(id);
        _clienteWriteOnlyRepository.Delete(cliente);
        await _unitOfWork.CommitAsync();
    }
}
