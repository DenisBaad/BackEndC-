using Aquiles.Communication.Requests.Faturas;

namespace Aquiles.Application.UseCases.Faturas.Create;
public interface ICreateFaturaUseCase
{
    public Task Execute(RequestCreateFaturaJson request);
}
