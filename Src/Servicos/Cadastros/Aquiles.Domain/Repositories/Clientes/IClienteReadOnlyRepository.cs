using Aquiles.Communication.Responses;
using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Clientes;
public interface IClienteReadOnlyRepository
{
    public Task<PagedResult<Cliente>> GetAll(Guid usuarioId, int pageNumber, int pageSize, string? search);
    public Task<Cliente> GetById(Guid id);
    public Task<bool> ExistClienteWithCode(int code);
}
