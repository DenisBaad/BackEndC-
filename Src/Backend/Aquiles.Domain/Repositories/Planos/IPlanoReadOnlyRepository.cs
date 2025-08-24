using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Planos;
public interface IPlanoReadOnlyRepository
{
    public Task<IList<Plano>> GetAll(Guid usuarioId);
    public Task<Plano> GetById(Guid id);
}
