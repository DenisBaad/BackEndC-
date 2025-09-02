using Aquiles.Communication.Requests.Planos;
using Bogus;

namespace CommonTestUtilities.Requests;
public class RequestCreatePlanoJsonBuilder
{
    public static RequestCreatePlanoJson Build()
    {
        return new Faker<RequestCreatePlanoJson>()
         .RuleFor(p => p.Descricao, f => f.Commerce.ProductName())
         .RuleFor(p => p.ValorPlano, f => f.Random.Decimal(1)) 
         .RuleFor(p => p.QuantidadeUsuarios, f => f.Random.Int(1)) 
         .RuleFor(p => p.VigenciaMeses, f => f.Random.Int(1));
    }
}
