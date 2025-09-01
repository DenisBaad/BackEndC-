namespace Enderecos.Domain.Entities;
public class Endereco : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Guid ClienteId { get; set; }
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Cep { get; set; }
    public string Municipio { get; set; }
    public string UF { get; set; }
    public bool Preferencial { get; set; } = false;
}
