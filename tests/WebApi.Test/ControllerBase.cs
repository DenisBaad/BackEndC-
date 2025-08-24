using Aquiles.Communication.Requests.Login;
using Aquiles.Exception;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace WebApi.Test;

public class ControllerBase : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public ControllerBase(CustomWebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        ResourceMensagensDeErro.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string metodo, object body, string token = "")
    {
        AuthorizeRequest(token);
        var jsonString = JsonConvert.SerializeObject(body);
        return await _httpClient.PostAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PutRequest(string metodo, object body, string token = "", Guid? id = null)
    {
        AuthorizeRequest(token);
        var url = id.HasValue ? $"{metodo}/{id}" : metodo;
        var jsonString = JsonConvert.SerializeObject(body);
        return await _httpClient.PutAsync(url, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    private void AuthorizeRequest(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    protected async Task<string> Login(string email, string senha)
    {
        var request = new RequestLoginJson { Email = email, Senha = senha };

        var response = await PostRequest("Login", request);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        return responseData.RootElement.GetProperty("token").GetString() ?? "";
    }
}

