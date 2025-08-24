using Aquiles.Communication.Responses.Clientes;

namespace Aquiles.Application.UseCases.Clientes.GetById;
public interface IGetIdClientesUseCase
{
    public Task<ResponseClientesJson> Execute(Guid id);
}
