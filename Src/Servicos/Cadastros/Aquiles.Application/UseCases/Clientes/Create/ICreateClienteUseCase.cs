using Aquiles.Communication.Requests.Clientes;
using Aquiles.Communication.Responses.Clientes;

namespace Aquiles.Application.UseCases.Clientes.Create;
public interface ICreateClienteUseCase
{
    public Task<ResponseClientesJson> Execute(RequestCreateClientesJson request);
}
