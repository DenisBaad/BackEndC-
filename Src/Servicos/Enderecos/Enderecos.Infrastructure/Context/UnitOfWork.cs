using Enderecos.Domain.Repositories;

namespace Enderecos.Infrastructure.Context;
public class UnitOfWork : IUnitOfWork
{
    private readonly EnderecosContext _context;

    public UnitOfWork(EnderecosContext context) => _context = context;

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}
