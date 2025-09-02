using Aquiles.Domain.Entities;
using Bogus;

namespace CommonTestUtilities.Entities;
public class PlanoBuilder
{
    public static Plano Build(Guid userId)
    {
        var planoGerado = new Faker<Plano>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.UsuarioId, _ => userId)
            .RuleFor(p => p.Descricao, f => f.Commerce.ProductName())
            .RuleFor(p => p.ValorPlano, f => f.Random.Decimal(1))
            .RuleFor(p => p.QuantidadeUsuarios, f => f.Random.Int(1))
            .RuleFor(p => p.VigenciaMeses, f => f.Random.Int(1));

        return planoGerado;
    }
}
