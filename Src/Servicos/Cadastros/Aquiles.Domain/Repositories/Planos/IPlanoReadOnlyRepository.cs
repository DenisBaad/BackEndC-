using Aquiles.Communication.Responses;
using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Planos;
public interface IPlanoReadOnlyRepository
{
    public Task<PagedResult<Plano>> GetAll(Guid usuarioId, int page, int pageSize, string? search);
    public Task<Plano> GetById(Guid id);
}
