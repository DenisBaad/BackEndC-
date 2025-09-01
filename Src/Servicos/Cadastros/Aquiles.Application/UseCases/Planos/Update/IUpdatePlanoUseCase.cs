using Aquiles.Communication.Requests.Planos;

namespace Aquiles.Application.UseCases.Planos.Update;
public interface IUpdatePlanoUseCase
{
    public Task Execute(RequestCreatePlanoJson request, Guid id);
}
