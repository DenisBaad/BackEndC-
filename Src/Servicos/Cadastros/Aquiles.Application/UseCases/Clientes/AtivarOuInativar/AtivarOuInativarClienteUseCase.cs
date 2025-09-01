using Aquiles.Communication.Enums;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;

namespace Aquiles.Application.UseCases.Clientes.AtivarOuInativar;
public class AtivarOuInativarClienteUseCase : IAtivarOuInativarClienteUseCase
{
    private readonly IClienteUpdateOnlyRepository _clienteUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtivarOuInativarClienteUseCase(
        IClienteUpdateOnlyRepository clienteUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _clienteUpdateOnlyRepository = clienteUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var cliente = await _clienteUpdateOnlyRepository.GetById(id);
        
        if(cliente.Status == EnumStatusCliente.Ativo)
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
}
