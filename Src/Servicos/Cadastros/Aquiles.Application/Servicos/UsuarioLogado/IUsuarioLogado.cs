namespace Aquiles.Application.Servicos.UsuarioLogado;
public interface IUsuarioLogado
{
    public Task<Guid?> GetUsuario();
}
