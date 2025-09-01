using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Usuarios;
public interface IUsuarioReadOnlyRepository
{
    public Task<bool> ExistUserByEmail(string email);
    public Task<Usuario> DoLogin(string email, string senha);
}
