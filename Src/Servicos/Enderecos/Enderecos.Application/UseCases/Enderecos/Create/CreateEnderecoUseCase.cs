using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Enderecos.Domain.Entities;
using Enderecos.Domain.Repositories;
using Enderecos.Domain.Repositories.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.Create;
public class CreateEnderecoUseCase : ICreateEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoWriteOnlyRepository _enderecoWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnderecoUseCase(
        IMapper mapper,
        IEnderecoWriteOnlyRepository enderecoWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _enderecoWriteRepository = enderecoWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEnderecoJson> Execute(RequestEnderecoJson request)
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
