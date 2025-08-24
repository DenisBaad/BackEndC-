using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Faturas;
public interface IFaturaReadOnlyRepository
{
    public Task<Fatura> GetById(Guid id);
    public Task<IList<Fatura>> GetAll(Guid? usuarioId);
    public Task<List<Fatura>> GetRelatorioFaturaPorCliente(Guid usuarioId, DateTime? dataAbertura, DateTime? dataFechamento, int? status, List<string> clientesIds);

}
