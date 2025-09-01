using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Planos;
public interface IPlanoWriteOnlyRepository
{
    public Task Create(Plano plano);
}
