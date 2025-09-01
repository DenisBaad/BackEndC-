namespace Aquiles.Utils.UsuarioLogado;
public interface IUsuarioLogado
{
    public Task<Guid?> GetUsuario();
}
