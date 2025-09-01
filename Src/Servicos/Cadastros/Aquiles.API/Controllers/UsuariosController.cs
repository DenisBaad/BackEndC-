using Aquiles.Application.UseCases.Usuarios.Create;
using Aquiles.Communication.Requests.Usuarios;
using Aquiles.Communication.Responses.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace Aquiles.API.Controllers;

public class UsuariosController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUsuariosJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromServices] ICreateUsuarioUseCase useCase, [FromBody] RequestCreateUsuariosJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}
