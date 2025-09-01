using Aquiles.Application.UseCases.Login.DoLogin;
using Aquiles.Communication.Requests.Login;
using Aquiles.Communication.Responses.Login;
using Microsoft.AspNetCore.Mvc;

namespace Aquiles.API.Controllers;

public class LoginController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var result = await useCase.Execute(request);
        return Ok(result);
    }
}
