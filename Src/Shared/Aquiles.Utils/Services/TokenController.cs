using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aquiles.Utils.Services;
public class TokenController
{
    private const string EmailAlias = "eml";
    private const string UsuarioLogado = "x5845";
    private readonly double _lifeTimeMinutesToken;
    private readonly string _securityKey;

    public TokenController(string securityKey, double lifeTimeMinutesToken)
    {
        _securityKey = securityKey;
        _lifeTimeMinutesToken = lifeTimeMinutesToken;
    }

    public string Create(string email, Guid idUsuario)
    {
        var claims = new List<Claim>() {
        new Claim(EmailAlias, email),
        new Claim(UsuarioLogado, idUsuario.ToString()),
    };

        var tokenHandler = new JwtSecurityTokenHandler();

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_lifeTimeMinutesToken),
            SigningCredentials = new SigningCredentials(SymmetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public string GetEmail(string token)
    {
        var claims = Validate(token);
        return claims.FindFirst(EmailAlias).Value;
    }

    public string GetUsuarioLogado(string token)
    {
        var claims = Validate(token);
        return claims.FindFirst(UsuarioLogado).Value;
    }

    public ClaimsPrincipal Validate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var parametrosValidacao = new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claims = tokenHandler.ValidateToken(token, parametrosValidacao, out _);
        return claims;
    }

    private SymmetricSecurityKey SymmetricKey()
    {
        return new SymmetricSecurityKey(Convert.FromBase64String(_securityKey));
    }
}
