using Aquiles.Communication.Requests.Planos;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Planos.Create;
public class CreatePlanoUseCase : ICreatePlanoUseCase
{
    private readonly IMapper _mapper;
    private readonly IPlanoWriteOnlyRepository _planoWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly ILogger<CreatePlanoUseCase> _logger;

    public CreatePlanoUseCase(
        IMapper mapper, 
        IPlanoWriteOnlyRepository planoWriteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado,
        ILogger<CreatePlanoUseCase> logger)
    {
        _planoWriteRepository = planoWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
        _logger = logger;
    }
    
    public async Task<ResponsePlanoJson> Execute(RequestCreatePlanoJson request)
    {
        try
        {
            var usuario = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

            Validate(request);
            var plano = _mapper.Map<Plano>(request);
            plano.Id = Guid.NewGuid();
            plano.UsuarioId = usuario;
            await _planoWriteRepository.Create(plano);
            await _unitOfWork.CommitAsync();

            return new ResponsePlanoJson()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar plano com request: {request}", request);
            throw;
        }
    }
    
    private void Validate(RequestCreatePlanoJson request)
    {
        var result = new PlanoValidator().Validate(request);
        
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
