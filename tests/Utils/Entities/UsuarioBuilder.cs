using Aquiles.Domain.Entities;
using Bogus;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;
public class UsuarioBuilder
{
    public static (Usuario usuario, string senha) Build()
    {
        string senha = string.Empty;
        var usuarioGerado = new Faker<Usuario>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Nome, f => f.Person.FirstName)
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.Senha, f =>
            {
                senha = f.Internet.Password();
                return PasswordEncryptBuilder.Build().Encript(senha);
            });

        return (usuarioGerado, senha);
    }
}
