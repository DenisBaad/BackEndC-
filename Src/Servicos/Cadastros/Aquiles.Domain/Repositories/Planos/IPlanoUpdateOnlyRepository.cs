using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Planos;
public interface IPlanoUpdateOnlyRepository
{
    public Task<Plano> GetById(Guid id);
    public void Update(Plano plano);
}
