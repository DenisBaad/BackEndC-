using Aquiles.Communication.Responses.Enderecos;
using AutoMapper;
using Enderecos.Domain.Repositories.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.GetAll;
public class GetAllEnderecoUseCase : IGetAllEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoReadOnlyRepository _readOnlyRepository;

    public GetAllEnderecoUseCase(
        IMapper mapper,
        IEnderecoReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
    }

    public async Task<IList<ResponseEnderecoJson>> Execute(Guid clienteId)
    {
        var endereco = await _readOnlyRepository.GetAll(clienteId);
        return _mapper.Map<IList<ResponseEnderecoJson>>(endereco);
    }
}
