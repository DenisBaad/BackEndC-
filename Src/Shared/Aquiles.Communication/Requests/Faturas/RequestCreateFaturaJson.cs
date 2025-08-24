using Aquiles.Communication.Enums;

namespace Aquiles.Communication.Requests.Faturas;
public class RequestCreateFaturaJson
{
    public Guid ClienteId { get; set; }
    public Guid PlanoId { get; set; }
    public EnumStatusFatura Status { get; set; }
    public DateTime InicioVigencia { get; set; }
    public DateTime FimVigencia { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataVencimento { get; set; }
}
