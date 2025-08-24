using Aquiles.Communication.Responses.Clientes;

namespace Aquiles.Application.UseCases.Clientes.GetAll;
public interface IGetAllClientesUseCase
{
    public Task<IList<ResponseClientesJson>> Execute();
}
