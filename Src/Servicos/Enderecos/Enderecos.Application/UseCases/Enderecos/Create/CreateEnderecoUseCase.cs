using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Enderecos.Domain.Entities;
using Enderecos.Domain.Repositories;
using Enderecos.Domain.Repositories.Enderecos;
using Microsoft.Extensions.Logging;

namespace Enderecos.Application.UseCases.Enderecos.Create;
public class CreateEnderecoUseCase : ICreateEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoWriteOnlyRepository _enderecoWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEnderecoUseCase> _logger;

    public CreateEnderecoUseCase(
        IMapper mapper,
        IEnderecoWriteOnlyRepository enderecoWriteRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateEnderecoUseCase> logger)
    {
        _enderecoWriteRepository = enderecoWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResponseEnderecoJson> Execute(RequestEnderecoJson request)
    {
        try
        {
            Validate(request);
            var endereco = _mapper.Map<Endereco>(request);
            endereco.Id = Guid.NewGuid();
            await _enderecoWriteRepository.Create(endereco);
            await _unitOfWork.CommitAsync();

            return new ResponseEnderecoJson()
            {
                Id = endereco.Id,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar endereço com request: {request}", request);
            throw;
        }
    }

    private void Validate(RequestEnderecoJson request)
    {
        var result = new EnderecoValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
