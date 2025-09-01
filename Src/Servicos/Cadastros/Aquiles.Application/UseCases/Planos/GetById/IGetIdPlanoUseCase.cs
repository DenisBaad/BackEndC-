using Aquiles.Communication.Responses.Planos;

namespace Aquiles.Application.UseCases.Planos.GetById;
public interface IGetIdPlanoUseCase
{
    public Task<ResponsePlanoJson> Execute(Guid id);
}
