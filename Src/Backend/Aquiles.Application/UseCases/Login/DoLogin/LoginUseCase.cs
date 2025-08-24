using Aquiles.Application.Servicos;
using Aquiles.Communication.Requests.Login;
using Aquiles.Communication.Responses.Login;
using Aquiles.Domain.Repositories.Usuarios;
using Aquiles.Exception.AquilesException;

namespace Aquiles.Application.UseCases.Login.DoLogin;
public class LoginUseCase : ILoginUseCase
{
    private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly PasswordEncrypt _passwordEncript;
    private readonly TokenController _tokenController;

    public LoginUseCase(
        IUsuarioReadOnlyRepository usuarioReadOnlyRepository, 
        PasswordEncrypt passwordEncript,
        TokenController tokenController)
    {
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _passwordEncript = passwordEncript;
        _tokenController = tokenController;
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
            if (ex is not InvalidLoginException)
            {
                throw new InvalidLoginException("E-mail ou senha inválidos.");
            }

            throw;
        }
    }
}
