using Aquiles.Communication.Requests.Enderecos;
using Bogus;

namespace CommonTestUtilities.Requests;
public class RequestCreateEnderecoJsonBuilder
{
    public static RequestEnderecoJson Build()
    {
        return new Faker<RequestEnderecoJson>()
            .RuleFor(p => p.Logradouro, f => f.Address.StreetName())
            .RuleFor(p => p.Numero, f => f.Address.BuildingNumber())
            .RuleFor(p => p.Complemento, f => f.Random.Bool() ? f.Address.SecondaryAddress() : null)
            .RuleFor(p => p.Bairro, f => f.Address.County())
            .RuleFor(p => p.Cep, f => f.Address.ZipCode())
            .RuleFor(p => p.Municipio, f => f.Address.City())
            .RuleFor(p => p.UF, f => f.Address.StateAbbr())
            .RuleFor(p => p.Preferencial, f => f.Random.Bool());
    }
}
