using Aquiles.Communication.Requests.Login;
using Aquiles.Communication.Responses.Login;
using Aquiles.Domain.Repositories.Usuarios;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.Services;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Login.DoLogin;
public class LoginUseCase : ILoginUseCase
{
    private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly PasswordEncrypt _passwordEncript;
    private readonly TokenController _tokenController;
    private readonly ILogger<LoginUseCase> _logger;

    public LoginUseCase(
        IUsuarioReadOnlyRepository usuarioReadOnlyRepository, 
        PasswordEncrypt passwordEncript,
        TokenController tokenController,
        ILogger<LoginUseCase> logger)
    {
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _passwordEncript = passwordEncript;
        _tokenController = tokenController;
        _logger = logger;
    }

    public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
    {
        try
        {
            var senhaCriptografada = _passwordEncript.Encript(request.Senha);
            var usuario = await _usuarioReadOnlyRepository.DoLogin(request.Email, senhaCriptografada);

            if (usuario is null)
            {
                throw new InvalidLoginException();
            }

            return new ResponseLoginJson()
            {
                Nome = usuario.Nome,
                Token = _tokenController.Create(usuario.Email, usuario.Id)
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar login para o email {request}", request.Email);
            throw;
        }
    }
}
