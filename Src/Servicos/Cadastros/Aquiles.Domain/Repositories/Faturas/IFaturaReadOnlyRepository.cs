using Aquiles.Communication.Responses;
using Aquiles.Domain.Entities;

namespace Aquiles.Domain.Repositories.Faturas;
public interface IFaturaReadOnlyRepository
{
    public Task<Fatura> GetById(Guid id);
    public Task<PagedResult<Fatura>> GetAll(Guid? usuarioId, int pageNumber, int pageSize);
    public Task<List<Fatura>> GetRelatorioFaturaPorCliente(Guid usuarioId, DateTime? dataAbertura, DateTime? dataFechamento, int? status, List<string> clientesIds);

}
