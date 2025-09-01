using Aquiles.Domain.Entities;
using Aquiles.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Planos.Create;
public class CreatePlanoTest : ControllerBase
{
    private const string METHOD = "Plano";
    private Usuario _usuario;
    private string _senha;

    public CreatePlanoTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.GetUsuario();
        _senha = factory.GetSenha();
    }

    [Fact]
    public async Task Sucesso()
    {
        var token = await Login(_usuario.Email, _senha);
        var request = RequestCreatePlanoJsonBuilder.Build();

        var response = await PostRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("descricao").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Descricao);
    }

    [Fact]
    public async Task Erro_Descricao_Vazia()
    {
        var token = await Login(_usuario.Email, _senha);
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.Descricao = string.Empty;

        var response = await PostRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(e => (e.GetString() ?? "").Equals(ResourceMensagensDeErro.DESCRICAO_OBRIGATORIA));
    }
}
