using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Faturas;
public interface IFaturaWriteOnlyRepository
{
    public Task Create(Fatura fatura);
}
