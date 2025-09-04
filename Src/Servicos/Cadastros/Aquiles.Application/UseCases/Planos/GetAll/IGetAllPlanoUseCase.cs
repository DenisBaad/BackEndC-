using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Planos;

namespace Aquiles.Application.UseCases.Planos.GetAll;
public interface IGetAllPlanoUseCase
{
    public Task<PagedResult<ResponsePlanoJson>> Execute(int pageNumber, int pageSize, string? search);
}

