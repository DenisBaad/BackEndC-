namespace Aquiles.Communication.Requests.Planos;
public class RequestCreatePlanoJson
{
    public string Descricao { get; set; }
    public decimal ValorPlano { get; set; }
    public int QuantidadeUsuarios { get; set; }
    public int VigenciaMeses { get; set; }
}
