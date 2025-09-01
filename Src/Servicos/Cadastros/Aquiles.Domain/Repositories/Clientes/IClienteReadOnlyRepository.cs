using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Clientes;
public interface IClienteReadOnlyRepository
{
    public Task<IList<Cliente>> GetAll(Guid usuarioId);
    public Task<Cliente> GetById(Guid id);
    public Task<bool> ExistClienteWithCode(int code);
}
