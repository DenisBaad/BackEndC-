using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Clientes;
public interface IClienteWriteOnlyRepository
{
    public Task AddAsync(Cliente cliente);
    public void Delete(Cliente cliente);
}
