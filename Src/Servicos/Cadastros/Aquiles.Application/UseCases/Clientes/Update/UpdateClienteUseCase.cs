using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories;
using Aquiles.Communication.Requests.Clientes;
using Aquiles.Exception.AquilesException;
using AutoMapper;

namespace Aquiles.Application.UseCases.Clientes.Update;
public class UpdateClienteUseCase : IUpdateClienteUseCase
{
    private readonly IClienteUpdateOnlyRepository _clienteUpdateOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClienteUseCase(IClienteUpdateOnlyRepository clienteUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _clienteUpdateOnlyRepository = clienteUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(Guid id, RequestCreateClientesJson request)
    {
        Validate(request);
        var cliente = await _clienteUpdateOnlyRepository.GetById(id);
        _mapper.Map(request, cliente);
        cliente.Id = id;
        _clienteUpdateOnlyRepository.Update(cliente);
        await _unitOfWork.CommitAsync();
    }

    public void Validate(RequestCreateClientesJson request)
    {
        var result = new ClienteValidator().Validate(request);

        if (!result.IsValid)
        {
            var mensagensDeErro = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorException(mensagensDeErro);
        }
    }
}
