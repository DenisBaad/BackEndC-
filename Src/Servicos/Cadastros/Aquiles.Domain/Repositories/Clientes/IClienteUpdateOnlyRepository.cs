using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Clientes;
public interface IClienteUpdateOnlyRepository
{
    public Task<Cliente> GetById(Guid id);
    public void Update(Cliente cliente);
}
