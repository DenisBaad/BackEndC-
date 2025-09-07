using Aquiles.Communication.Requests.Planos;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Planos.Update;
public class UpdatePlanoUseCase : IUpdatePlanoUseCase
{
    private readonly IPlanoUpdateOnlyRepository _planoUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePlanoUseCase> _logger;

    public UpdatePlanoUseCase(
        IPlanoUpdateOnlyRepository planoUpdateOnlyRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ILogger<UpdatePlanoUseCase> logger)
    {
        _planoUpdateOnlyRepository = planoUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task Execute(RequestCreatePlanoJson request, Guid id)
    {
        try
        {
            Validate(request);
            var plano = await _planoUpdateOnlyRepository.GetById(id);
            _mapper.Map(request, plano);
            plano.Id = id;
            _planoUpdateOnlyRepository.Update(plano);
            await _unitOfWork.CommitAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar plano com request: {request}", request);
            throw;
        }
    }
    
    private void Validate(RequestCreatePlanoJson request)
    {
        var validator = new PlanoValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
