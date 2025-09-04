using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Faturas;

namespace Aquiles.Application.UseCases.Faturas.GetAll;
public interface IGetAllFaturaUseCase
{
    public Task<PagedResult<ResponseFaturaJson>> Execute(int pageNumber, int pageSize);
}
