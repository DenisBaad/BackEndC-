using Aquiles.Communication.Requests.Clientes;

namespace Aquiles.Application.UseCases.Clientes.Update;
public interface IUpdateClienteUseCase
{
    public Task Execute(Guid id, RequestCreateClientesJson request);
}
