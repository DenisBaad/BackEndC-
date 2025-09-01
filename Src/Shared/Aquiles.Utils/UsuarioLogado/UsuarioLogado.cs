using Aquiles.Utils.Services;
using Microsoft.AspNetCore.Http;

namespace Aquiles.Utils.UsuarioLogado;
public class UsuarioLogado : IUsuarioLogado
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenController _tokenController;

    public UsuarioLogado(IHttpContextAccessor httpContextAccessor, TokenController tokenController)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenController = tokenController;
    }

    public async Task<Guid?> GetUsuario()
    {
        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorization["Bearer".Length..].Trim();
        return Guid.Parse(_tokenController.GetUsuarioLogado(token));
    }
}
