using Aquiles.Communication.Requests.Usuarios;
using Bogus;

namespace CommonTestUtilities.Requests;
public class RequestCreateUsuariosJsonBuilder
{
    public static RequestCreateUsuariosJson Build(int senha = 10)
    {
        return new Faker<RequestCreateUsuariosJson>()
            .RuleFor(x => x.Nome, (f) => f.Person.FirstName)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Nome))
            .RuleFor(x => x.Senha, (f) => f.Internet.Password(senha));
    }
}
