namespace Aquiles.Domain.Entities;
public class Plano : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public string Descricao { get; set; }
    public decimal ValorPlano { get; set; }
    public int QuantidadeUsuarios { get; set; }
    public int VigenciaMeses { get; set; }
}