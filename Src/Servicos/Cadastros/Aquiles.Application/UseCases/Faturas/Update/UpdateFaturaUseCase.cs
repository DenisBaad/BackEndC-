using Aquiles.Communication.Requests.Faturas;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Faturas.Update;
public class UpdateFaturaUseCase : IUpdateFaturaUseCase
{
    private readonly IFaturaUpdateOnlyRepository _faturaUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateFaturaUseCase> _logger;

    public UpdateFaturaUseCase(
        IFaturaUpdateOnlyRepository faturaUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateFaturaUseCase> logger)
    {
        _faturaUpdateOnlyRepository = faturaUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task Execute(RequestCreateFaturaJson request, Guid id)
    {
        try
        {
            Validate(request);
            var fatura = await _faturaUpdateOnlyRepository.GetById(id);
            _mapper.Map(request, fatura);
            fatura.Id = id;
            _faturaUpdateOnlyRepository.Update(fatura);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar fatura com request: {request}", request);
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
