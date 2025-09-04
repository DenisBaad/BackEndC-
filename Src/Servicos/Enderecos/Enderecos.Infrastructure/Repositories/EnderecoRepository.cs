using Enderecos.Domain.Entities;
using Enderecos.Domain.Repositories.Enderecos;
using Enderecos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Enderecos.Infrastructure.Repositories;
public class EnderecoRepository : IEnderecoWriteOnlyRepository, IEnderecoReadOnlyRepository
{
    private readonly EnderecosContext _context;

    public EnderecoRepository(EnderecosContext context) => _context = context;

    public async Task Create(Endereco endereco) => await _context.Enderecos.AddAsync(endereco);

    public async Task<IList<Endereco>> GetAll(Guid clienteId) => await _context.Enderecos.AsNoTracking().Where(x => x.ClienteId == clienteId).ToListAsync();
}
