using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Usuarios;
public interface IUsuarioWriteOnlyRepository
{
    public Task AddAsync(Usuario usuario);
}
