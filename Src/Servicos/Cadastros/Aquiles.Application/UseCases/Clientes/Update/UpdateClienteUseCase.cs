using Aquiles.Communication.Requests.Clientes;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Clientes.Update;
public class UpdateClienteUseCase : IUpdateClienteUseCase
{
    private readonly IClienteUpdateOnlyRepository _clienteUpdateOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateClienteUseCase> _logger;

    public UpdateClienteUseCase(IClienteUpdateOnlyRepository clienteUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateClienteUseCase> logger)
    {
        _clienteUpdateOnlyRepository = clienteUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Execute(Guid id, RequestCreateClientesJson request)
    {
        try
        {
            Validate(request);
            var cliente = await _clienteUpdateOnlyRepository.GetById(id);
            _mapper.Map(request, cliente);
            cliente.Id = id;
            _clienteUpdateOnlyRepository.Update(cliente);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar cliente com request: {request}", request);
            throw;
        }
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
