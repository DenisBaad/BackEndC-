using Enderecos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enderecos.Infrastructure.Context;
public class EnderecosContext : DbContext
{
    public EnderecosContext(DbContextOptions<EnderecosContext> options) : base(options) { }

    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnderecosContext).Assembly);
    }
}
