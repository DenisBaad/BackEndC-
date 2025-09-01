namespace Enderecos.Domain.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
