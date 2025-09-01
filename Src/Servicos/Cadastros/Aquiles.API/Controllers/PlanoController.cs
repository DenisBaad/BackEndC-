using Aquiles.Application.UseCases.Planos.Create;
using Aquiles.Application.UseCases.Planos.GetAll;
using Aquiles.Application.UseCases.Planos.GetById;
using Aquiles.Application.UseCases.Planos.Update;
using Aquiles.Communication.Requests.Planos;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Utils.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Aquiles.API.Controllers;

[ServiceFilter(typeof(AquilesAuthorize))]

public class PlanoController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponsePlanoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromServices] ICreatePlanoUseCase useCase, [FromBody] RequestCreatePlanoJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IList<ResponsePlanoJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromServices] IGetAllPlanoUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponsePlanoJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetId([FromServices] IGetIdPlanoUseCase useCase, [FromRoute] Guid id)
    {
        var result = await useCase.Execute(id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar([FromServices] IUpdatePlanoUseCase useCase, [FromRoute] Guid id, [FromBody] RequestCreatePlanoJson plano)
    {
        await useCase.Execute(plano, id);
        return NoContent();
    }
}
