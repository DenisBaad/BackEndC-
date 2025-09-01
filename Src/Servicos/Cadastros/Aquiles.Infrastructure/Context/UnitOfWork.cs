using Aquiles.Domain.Repositories;

namespace Aquiles.Infrastructure.Context;
public class UnitOfWork : IUnitOfWork
{
    private readonly AquilesContext _context;

    public UnitOfWork(AquilesContext context) => _context = context;

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}
