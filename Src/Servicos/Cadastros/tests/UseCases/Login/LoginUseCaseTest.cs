using Aquiles.Application.UseCases.Login.DoLogin;
using Aquiles.Communication.Requests.Login;
using Aquiles.Domain.Entities;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Usuarios;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login;
public class LoginUseCaseTest
{
    [Fact]
    public async Task Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Build();
        var request = new RequestLoginJson()
        {
            Email = usuario.Email,
            Senha = senha
        };
        var useCase = CriarUseCase(usuario);
        
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Nome.Should().Be(usuario.Nome);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    private LoginUseCase CriarUseCase(Usuario usuario)
    {
        var repository = new UsuarioReadOnlyRepositoryBuilder().DoLogin(usuario).Build();
        var encriptador = PasswordEncryptBuilder.Build();
        var token = TokenControllerBuilder.Build();
        return new LoginUseCase(repository, encriptador, token);
    }

    [Fact]
    public async Task Erro_Login_Senha_Incorreta()
    {
        (var usuario, var senha) = UsuarioBuilder.Build();
        var request = new RequestLoginJson()
        {
            Email = usuario.Email,
            Senha = "SenhaInvalida"
        };
        var useCase = CriarUseCase(usuario);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<InvalidLoginException>())
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Erro_Login_Email_Incorreto()
    {
        (var usuario, var senha) = UsuarioBuilder.Build();
        var request = new RequestLoginJson()
        {
            Email = "email@incorreto.com",
            Senha = senha
        };
        var useCase = CriarUseCase(usuario);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<InvalidLoginException>())
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Erro_Login_Senha_E_Email_Incorretos()
    {
        (var usuario, var senha) = UsuarioBuilder.Build();
        var request = new RequestLoginJson()
        {
            Email = "email@incorreto.com",
            Senha = "SenhaInvalida"
        };
        var useCase = CriarUseCase(usuario);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<InvalidLoginException>())
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }
}
