using System.Globalization;

namespace Aquiles.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _locales = new List<string>()
    {
        "pt-BR",
        "es"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var culture = new CultureInfo("pt-BR");
        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var linguagem = context.Request.Headers["Accept-Language"].ToString();
            if (_locales.Any(c => c.Equals(linguagem)))
            {
                culture = new CultureInfo(linguagem);
            }
        }
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
