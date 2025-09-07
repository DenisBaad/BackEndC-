using Aquiles.Communication.Requests.Faturas;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Faturas.Create;
public class CreateFaturaUseCase : ICreateFaturaUseCase
{
    private readonly IMapper _mapper;
    private readonly IFaturaWriteOnlyRepository _faturaWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly ILogger<CreateFaturaUseCase> _logger;

    public CreateFaturaUseCase(
        IMapper mapper, 
        IFaturaWriteOnlyRepository faturaWriteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado,
        ILogger<CreateFaturaUseCase> logger)
    {
        _faturaWriteRepository = faturaWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
        _logger = logger;
    }
    
    public async Task Execute(RequestCreateFaturaJson request)
    {
        try
        {
            var usuario = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

            Validate(request);
            var fatura = _mapper.Map<Fatura>(request);
            fatura.Id = Guid.NewGuid();
            fatura.UsuarioId = usuario;
            await _faturaWriteRepository.Create(fatura);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fatura com request: {request}", request);
            throw;
        }
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
