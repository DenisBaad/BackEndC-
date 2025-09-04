using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;
using Aquiles.Utils.Filters;
using Enderecos.Application.UseCases.Enderecos.Create;
using Enderecos.Application.UseCases.Enderecos.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Enderecos.API.Controllers;

[ServiceFilter(typeof(AquilesAuthorize))]

public class EnderecosController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseEnderecoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromServices] ICreateEnderecoUseCase useCase, [FromBody] RequestEnderecoJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet("{clienteId}")]
    [ProducesResponseType(typeof(IList<ResponseEnderecoJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllEnderecoUseCase useCase, [FromRoute] Guid clienteId)
    {
        var result = await useCase.Execute(clienteId);
        return Ok(result);
    }
}
