using Aquiles.Domain.Entities;
using Aquiles.Infrastructure.Context;
using CommonTestUtilities.Entities;

namespace WebApi.Test;
public class ContextSeedInMemory
{
    public static (Usuario usuario, string senha, Plano plano) Seed(AquilesContext context)
    {
        var (usuario, senha) = SeedUsuario(context);
        var plano = SeedPlano(context, usuario.Id);
        return (usuario, senha, plano);
    }

    public static (Usuario usuario, string senha) SeedUsuario(AquilesContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.Build();
        context.Usuarios.Add(usuario);
        context.SaveChanges();
        return (usuario, senha);
    }

    public static Plano SeedPlano(AquilesContext context, Guid usuarioId)
    {
        var plano = PlanoBuilder.Build(usuarioId);
        context.Planos.Add(plano);
        context.SaveChanges();
        return plano;
    }
}
