using Aquiles.Communication.Responses;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Aquiles.Utils.Services;

namespace Aquiles.Utils.Filters;
public class AquilesAuthorize : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;

    public AquilesAuthorize(TokenController tokenController) => _tokenController = tokenController;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = RequestToken(context);
            var email = _tokenController.GetEmail(token);
            var usuario = _tokenController.GetUsuarioLogado(token) ?? throw new AquilesException("O usuário não existe");
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch
        {
            NoPermission(context);
        }
    }

    private string RequestToken(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new AquilesException("Token não informado");
        }
        return authorization["Bearer".Length..].Trim();
    }

    private void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMensagensDeErro.TOKEN_EXPIRADO));
    }

    private void NoPermission(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMensagensDeErro.USUARIO_SEM_PERMISSAO));
    }
}
