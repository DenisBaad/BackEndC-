using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Repositories.Clientes;
using AutoMapper;

namespace Aquiles.Application.UseCases.Clientes.GetById;
public class GetIdClientesUseCase : IGetIdClientesUseCase
{
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    
    public GetIdClientesUseCase(
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper)
    {
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
    }

    public async Task<ResponseClientesJson> Execute(Guid id)
    {
        var cliente = await _clienteReadOnlyRepository.GetById(id);
        var response = _mapper.Map<ResponseClientesJson>(cliente);
        return response;
    }
}
