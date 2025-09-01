using Aquiles.Communication.Requests.Usuarios;
using Aquiles.Communication.Responses.Usuarios;

namespace Aquiles.Application.UseCases.Usuarios.Create;
public interface ICreateUsuarioUseCase
{
    public Task<ResponseUsuariosJson> Execute(RequestCreateUsuariosJson request);
}
