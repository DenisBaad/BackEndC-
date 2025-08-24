using Aquiles.Communication.Responses.Planos;

namespace Aquiles.Application.UseCases.Planos.GetAll;
public interface IGetAllPlanoUseCase
{
    public Task<IList<ResponsePlanoJson>> Execute();
}

