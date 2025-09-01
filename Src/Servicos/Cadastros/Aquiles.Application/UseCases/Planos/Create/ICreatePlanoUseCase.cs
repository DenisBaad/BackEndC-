using Aquiles.Communication.Requests.Planos;
using Aquiles.Communication.Responses.Planos;

namespace Aquiles.Application.UseCases.Planos.Create;
public interface ICreatePlanoUseCase
{
    public Task<ResponsePlanoJson> Execute(RequestCreatePlanoJson request);
}
