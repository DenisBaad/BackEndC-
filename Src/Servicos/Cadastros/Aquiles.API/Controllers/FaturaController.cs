using Aquiles.Application.Servicos;
using Aquiles.Application.UseCases.Faturas.Create;
using Aquiles.Application.UseCases.Faturas.GetAll;
using Aquiles.Application.UseCases.Faturas.Update;
using Aquiles.Application.UseCases.Relatorios.RelatorioFaturas;
using Aquiles.Communication.Enums;
using Aquiles.Communication.Requests.Faturas;
using Aquiles.Communication.Responses.Faturas;
using Microsoft.AspNetCore.Mvc;

namespace Aquiles.API.Controllers;

[ServiceFilter(typeof(AquilesAuthorize))]
public class FaturaController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromServices] ICreateFaturaUseCase useCase, [FromBody] RequestCreateFaturaJson request)
    {
        await useCase.Execute(request);
        return Created(string.Empty, null);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IList<ResponseFaturaJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromServices] IGetAllFaturaUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar([FromServices] IUpdateFaturaUseCase useCase, [FromRoute] Guid id, [FromBody] RequestCreateFaturaJson fatura)
    {
        await useCase.Execute(fatura, id);
        return NoContent();
    }

    [HttpGet("gerar-relatorio-faturas-clientes")]
    [ProducesResponseType(typeof(ResponseFaturaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GerarRelatorioFaturasPorCliente(
        [FromQuery] string usuarioNome,
        [FromQuery] DateTime? dataAbertura,
        [FromQuery] DateTime? dataFechamento,
        [FromQuery] EnumStatusFatura? status,
        [FromQuery] List<string> clienteId,
        [FromServices] IRelatorioFaturas useCase)
    {
        var pdfBytes = await useCase.Executar(usuarioNome, dataAbertura, dataFechamento, status, clienteId);
        return File(pdfBytes, "application/pdf", "relatorio_faturas.pdf");
    }
}
