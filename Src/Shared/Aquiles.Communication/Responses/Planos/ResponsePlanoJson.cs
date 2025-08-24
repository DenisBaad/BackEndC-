namespace Aquiles.Communication.Responses.Planos;
public class ResponsePlanoJson
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public decimal ValorPlano { get; set; }
    public int QuantidadeUsuarios { get; set; }
    public int VigenciaMeses { get; set; }
}
