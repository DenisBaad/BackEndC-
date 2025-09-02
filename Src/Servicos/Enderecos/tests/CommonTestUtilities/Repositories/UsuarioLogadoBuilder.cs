using Aquiles.Utils.UsuarioLogado;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UsuarioLogadoBuilder
{
    public static IUsuarioLogado Build()
    {
        var mock = new Mock<IUsuarioLogado>();
        mock.Setup(x => x.GetUsuario()).ReturnsAsync(Guid.NewGuid());
        return mock.Object;
    }
}
