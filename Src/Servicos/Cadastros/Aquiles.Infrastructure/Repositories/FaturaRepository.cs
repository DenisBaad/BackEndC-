using Aquiles.Communication.Enums;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Aquiles.Infrastructure.Repositories;
public class FaturaRepository : IFaturaWriteOnlyRepository, IFaturaReadOnlyRepository, IFaturaUpdateOnlyRepository
{
    private readonly AquilesContext _context;
    public FaturaRepository(AquilesContext context) => _context = context;

    public async Task Create(Fatura fatura) => await _context.Faturas.AddAsync(fatura);

    public async Task<Fatura> GetById(Guid id) => await _context.Faturas.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IList<Fatura>> GetAll(Guid? usuarioId) => await _context.Faturas.AsNoTracking().Where(x => x.UsuarioId == usuarioId).Include(p => p.Plano).Include(c => c.Cliente).ToListAsync();

    async Task<Fatura> IFaturaUpdateOnlyRepository.GetById(Guid id) => await _context.Faturas.FirstOrDefaultAsync(x => x.Id == id);

    public void Update(Fatura fatura) => _context.Faturas.Update(fatura);

    public void Delete(Fatura fatura) => _context.Faturas.Remove(fatura);

    public async Task<List<Fatura>> GetRelatorioFaturaPorCliente(Guid usuarioId, DateTime? dataAbertura, DateTime? dataFechamento, int? status, List<string> clientesIds)
    {
        var query = _context.Faturas.AsNoTracking().Where(p => p.UsuarioId == usuarioId);

        if (dataAbertura.HasValue)
        {
            query = query.Where(p => p.InicioVigencia >= dataAbertura.Value);
        }

        if (dataFechamento.HasValue)
        {
            var fimDoDia = dataFechamento.Value.Date.AddDays(1).AddMilliseconds(-1);
            query = query.Where(p => p.FimVigencia <= fimDoDia);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == (EnumStatusFatura)status.Value);
        }

        if (clientesIds != null && clientesIds.Any())
        {
            query = query.Where(p => clientesIds.Contains(p.ClienteId.ToString()));
        }

        return await query.ToListAsync();
    }
}
