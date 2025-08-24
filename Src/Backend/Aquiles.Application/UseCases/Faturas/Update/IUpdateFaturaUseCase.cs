using Aquiles.Communication.Requests.Faturas;

namespace Aquiles.Application.UseCases.Faturas.Update;
public interface IUpdateFaturaUseCase
{
    public Task Execute(RequestCreateFaturaJson request, Guid id);
}
