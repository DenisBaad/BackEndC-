using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Repositories.Clientes;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Clientes.GetById;
public class GetIdClientesUseCase : IGetIdClientesUseCase
{
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetIdClientesUseCase> _logger;

    public GetIdClientesUseCase(
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper,
        ILogger<GetIdClientesUseCase> logger)
    {
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResponseClientesJson> Execute(Guid id)
    {
        try
        {
            var cliente = await _clienteReadOnlyRepository.GetById(id);
            var response = _mapper.Map<ResponseClientesJson>(cliente);
            return response;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar clientes com clienteId: {clienteId}", id);
            throw;
        }
    }
}
