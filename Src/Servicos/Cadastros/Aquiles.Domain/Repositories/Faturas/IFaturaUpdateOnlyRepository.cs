using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Faturas;
public interface IFaturaUpdateOnlyRepository
{
    public Task<Fatura> GetById(Guid id);
    public void Update(Fatura fatura);
}
