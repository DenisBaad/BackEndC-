using Aquiles.Communication.Responses.Enderecos;
using AutoMapper;
using Enderecos.Domain.Repositories.Enderecos;
using Microsoft.Extensions.Logging;

namespace Enderecos.Application.UseCases.Enderecos.GetAll;
public class GetAllEnderecoUseCase : IGetAllEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoReadOnlyRepository _readOnlyRepository;
    private readonly ILogger<GetAllEnderecoUseCase> _logger;

    public GetAllEnderecoUseCase(
        IMapper mapper,
        IEnderecoReadOnlyRepository readOnlyRepository,
        ILogger<GetAllEnderecoUseCase> logger)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IList<ResponseEnderecoJson>> Execute(Guid clienteId)
    {
        try
        {
            var endereco = await _readOnlyRepository.GetAll(clienteId);
            return _mapper.Map<IList<ResponseEnderecoJson>>(endereco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar endereços com clienteId: {clienteId}", clienteId);
            throw;
        }
    }
}
