using Aquiles.Communication.Requests.Login;
using Aquiles.Communication.Responses.Login;

namespace Aquiles.Application.UseCases.Login.DoLogin;
public interface ILoginUseCase
{
    public Task<ResponseLoginJson> Execute(RequestLoginJson request);
}
