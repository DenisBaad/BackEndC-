using Aquiles.Communication.Requests.Faturas;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;

namespace Aquiles.Application.UseCases.Faturas.Create;
public class CreateFaturaUseCase : ICreateFaturaUseCase
{
    private readonly IMapper _mapper;
    private readonly IFaturaWriteOnlyRepository _faturaWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    
    public CreateFaturaUseCase(
        IMapper mapper, 
        IFaturaWriteOnlyRepository faturaWriteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado)
    {
        _faturaWriteRepository = faturaWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
    }
    
    public async Task Execute(RequestCreateFaturaJson request)
    {
        var usuario = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        Validate(request);
        var fatura = _mapper.Map<Fatura>(request);
        fatura.Id = Guid.NewGuid();
        fatura.UsuarioId = usuario;
        await _faturaWriteRepository.Create(fatura);
        await _unitOfWork.CommitAsync();
    }
    
    private void Validate(RequestCreateFaturaJson request)
    {
        var validator = new FaturaValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
