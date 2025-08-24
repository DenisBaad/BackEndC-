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

    public async Task<IList<Cliente>> GetAll(Guid usuarioId) => await _context.Clientes.AsNoTracking().Where(x =>x.UsuarioId == usuarioId).ToListAsync();

    async Task<Cliente> IClienteReadOnlyRepository.GetById(Guid id) => await _context.Clientes.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

    public void Update(Cliente cliente) => _context.Clientes.Update(cliente);

    async Task<Cliente> IClienteUpdateOnlyRepository.GetById(Guid id) => await _context.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();

    public void Delete(Cliente cliente)  => _context.Clientes.Remove(cliente);
}
