using Aquiles.Communication.Requests.Enderecos;

namespace Aquiles.Communication.Contracts;
public class ClienteEvent
{
    public Guid ClienteId { get; set; }
    public Guid UsuarioId { get; set; }
    public RequestEnderecoJson Endereco { get; set; }
}
