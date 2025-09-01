using Aquiles.Communication.Requests.Planos;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Domain.Repositories;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Utils.UsuarioLogado;

namespace Aquiles.Application.UseCases.Planos.Create;
public class CreatePlanoUseCase : ICreatePlanoUseCase
{
    private readonly IMapper _mapper;
    private readonly IPlanoWriteOnlyRepository _planoWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    
    public CreatePlanoUseCase(
        IMapper mapper, 
        IPlanoWriteOnlyRepository planoWriteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado)
    {
        _planoWriteRepository = planoWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
    }
    
    public async Task<ResponsePlanoJson> Execute(RequestCreatePlanoJson request)
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
