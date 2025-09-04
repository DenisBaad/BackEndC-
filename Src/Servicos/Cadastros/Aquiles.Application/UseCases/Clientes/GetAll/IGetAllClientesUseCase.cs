using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Clientes;

namespace Aquiles.Application.UseCases.Clientes.GetAll;
public interface IGetAllClientesUseCase
{
    public Task<PagedResult<ResponseClientesJson>> Execute(int pageNumber, int pageSize, string? search);
}
