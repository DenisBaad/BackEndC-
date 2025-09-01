using Aquiles.Communication.Requests.Faturas;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Domain.Repositories;
using Aquiles.Exception.AquilesException;
using AutoMapper;

namespace Aquiles.Application.UseCases.Faturas.Update;
public class UpdateFaturaUseCase : IUpdateFaturaUseCase
{
    private readonly IFaturaUpdateOnlyRepository _faturaUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdateFaturaUseCase(
        IFaturaUpdateOnlyRepository faturaUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _faturaUpdateOnlyRepository = faturaUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Execute(RequestCreateFaturaJson request, Guid id)
    {
        Validate(request);
        var fatura = await _faturaUpdateOnlyRepository.GetById(id);
        _mapper.Map(request, fatura);
        fatura.Id = id;
        _faturaUpdateOnlyRepository.Update(fatura);
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
