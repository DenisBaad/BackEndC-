using Aquiles.Communication.Requests.Planos;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Domain.Repositories;
using Aquiles.Exception.AquilesException;
using AutoMapper;

namespace Aquiles.Application.UseCases.Planos.Update;
public class UpdatePlanoUseCase : IUpdatePlanoUseCase
{
    private readonly IPlanoUpdateOnlyRepository _planoUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdatePlanoUseCase(
        IPlanoUpdateOnlyRepository planoUpdateOnlyRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _planoUpdateOnlyRepository = planoUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Execute(RequestCreatePlanoJson request, Guid id)
    {
        Validate(request);
        var plano = await _planoUpdateOnlyRepository.GetById(id);
        _mapper.Map(request, plano);
        plano.Id = id;
        _planoUpdateOnlyRepository.Update(plano);
        await _unitOfWork.CommitAsync();
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
