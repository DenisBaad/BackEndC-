using Aquiles.Communication.Responses;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Aquiles.Infrastructure.Repositories;
public class ClienteRepository : IClienteWriteOnlyRepository, IClienteReadOnlyRepository, IClienteUpdateOnlyRepository
{
    private readonly AquilesContext _context;

    public ClienteRepository(AquilesContext context) => _context = context;

    public async Task AddAsync(Cliente cliente) => await _context.Clientes.AddAsync(cliente);

    public async Task<bool> ExistClienteWithCode(int code) => await _context.Clientes.AsNoTracking().AnyAsync(x => x.Codigo.Equals(code));

    public async Task<PagedResult<Cliente>> GetAll(Guid usuarioId, int pageNumber, int pageSize, string? search)
    {
        var query = _context.Clientes.AsNoTracking().Where(x => x.UsuarioId == usuarioId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Nome.Contains(search));

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Cliente> { Items = items, TotalCount = totalCount };
    }

    async Task<Cliente> IClienteReadOnlyRepository.GetById(Guid id) => await _context.Clientes.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

    public void Update(Cliente cliente) => _context.Clientes.Update(cliente);

    async Task<Cliente> IClienteUpdateOnlyRepository.GetById(Guid id) => await _context.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();

    public void Delete(Cliente cliente)  => _context.Clientes.Remove(cliente);
}
