using Aquiles.Communication.Responses.Faturas;

namespace Aquiles.Application.UseCases.Faturas.GetAll;
public interface IGetAllFaturaUseCase
{
    public Task<IList<ResponseFaturaJson>> Execute();
}
