using Aquiles.Communication.Enums;

namespace Aquiles.Domain.Entities;
public class Cliente : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public int Codigo { get; set; }
    public EnumTipoCliente Tipo { get; set; }
    public string? CpfCnpj { get; set; }
    public EnumStatusCliente Status { get; set; }
    public string Nome { get; set; }
    public string? Identidade { get; set; }
    public string? OrgaoExpedidor { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? NomeFantasia { get; set; }
    public string Contato { get; set; }
}
