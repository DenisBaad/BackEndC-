using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Aquiles.Infrastructure.Repositories;
public class PlanoRepository : IPlanoWriteOnlyRepository, IPlanoReadOnlyRepository, IPlanoUpdateOnlyRepository
{
    private readonly AquilesContext _context;
    public PlanoRepository(AquilesContext context) => _context = context;

    public async Task Create(Plano plano) => await _context.Planos.AddAsync(plano);

    public async Task<Plano> GetById(Guid id) => await _context.Planos.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<IList<Plano>> GetAll(Guid usuarioId) => await _context.Planos.AsNoTracking().Where(x => x.UsuarioId == usuarioId).ToListAsync();
    
    async Task<Plano> IPlanoUpdateOnlyRepository.GetById(Guid id) => await _context.Planos.FirstOrDefaultAsync(x => x.Id == id);

    public void Update(Plano plano) => _context.Planos.Update(plano);

    public void Delete(Plano plano) => _context.Planos.Remove(plano);
}


