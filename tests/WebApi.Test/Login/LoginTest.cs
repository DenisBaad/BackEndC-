using Aquiles.Communication.Requests.Login;
using Aquiles.Domain.Entities;
using Aquiles.Exception;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Login;
public class LoginTest : ControllerBase
{
    private const string METHOD = "Login";
    private Usuario _usuario;
    private string _senha;
    
    public LoginTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.GetUsuario();
        _senha = factory.GetSenha();
    }

    [Fact]
    public async Task Sucesso()
    {
        var request = new RequestLoginJson { Email = _usuario.Email, Senha = _senha };

        var response = await PostRequest(METHOD, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("nome").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Erro_Validar_Email_Invalido()
    {
        var request = new RequestLoginJson { Email = "email@invalido.com", Senha = _senha };

        var response = await PostRequest(METHOD, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Erro_Validar_Senha_Invalida()
    {
        var request = new RequestLoginJson { Email = _usuario.Email, Senha = "senhaInvalida" };

        var response = await PostRequest(METHOD, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Erro_Validar_Email_E_Senha_Invalidos()
    {
        var request = new RequestLoginJson { Email = "email@invalido.com", Senha = "senhaInvalida" };

        var response = await PostRequest(METHOD, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }
}
