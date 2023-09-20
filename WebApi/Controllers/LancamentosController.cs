using Domain.Interfaces.ILancamento;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class LancamentosController : ControllerBase
{
    private readonly InterfaceLancamento _repository;
    private readonly ILancamentoServico _service;

    public LancamentosController(InterfaceLancamento repository, ILancamentoServico service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet("ListarLancamentosUsuario")]
    [Produces("application/json")]
    public async Task<object> ListarLancamentosUsuario(string email) => await _repository.ListaLancamentosUsuario(email);

    [HttpGet("ObterLancamento/{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<Lancamento>> ObterLancamento(int id)
    {
        Lancamento lancamento = await _repository.GetEntityById(id);
        return Ok(lancamento);
    }

    [HttpGet("CarregarGraficos")]
    [Produces("application/json")]
    public async Task<object> CarregarGraficos(string email) => await _service.CarregaGraficos(email);

    [HttpPost("AdicionarLancamento")]
    [Produces("application/json")]
    public async Task<ActionResult<Lancamento>> AdicionarLancamento(Lancamento lancamento)
    {
        await _service.AdicionarLancamento(lancamento);
        return Ok(lancamento);
    }

    [HttpPost("ImportarLancamentosExtratoBradescoCSV/{idCategoria:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> ImportarLancamentosExtratoBradescoCSV(IFormFile arquivo, int idCategoria)
    {
        using var streamReader = new StreamReader(arquivo.OpenReadStream());
        await _service.ImportarLancamentosExtratoBradescoCSV(streamReader, idCategoria);
        return Ok();
    }

    [HttpPost("ImportarLancamentosExtratoItauCSV/{idCategoria:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> ImportarLancamentosExtratoItauCSV(IFormFile arquivo, int idCategoria)
    {
        // criar uma instância do Workbook
        IWorkbook workbook;
        using (Stream stream = arquivo.OpenReadStream())
        {
            workbook = new HSSFWorkbook(stream);
        }
        
        await _service.ImportarLancamentosExtratoItauCSV(workbook, idCategoria);
        
        return Ok();
    }

    [HttpPost("ImportarNotaFiscal/{idCategoria:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> ImportarNotaFiscal(List<IFormFile> arquivos, int idCategoria)
    {
        try
        {
            foreach (var arquivo in arquivos)
            {
                using (StreamReader reader = new StreamReader(arquivo.OpenReadStream()))
                {
                    string xmlContent = await reader.ReadToEndAsync();

                    await _service.ImportarNotaFiscal(xmlContent, idCategoria);
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao importar a nota fiscal: {ex.Message}");
        }
    }

    [HttpPut("AtualizarLancamento")]
    [Produces("application/json")]
    public async Task<ActionResult<Lancamento>> AtualizarLancamento(Lancamento lancamento)
    {
        await _service.AtualizarLancamento(lancamento);
        return Ok(lancamento);
    }

    [HttpDelete("DeletarLancamento/{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarLancamento(int id)
    {
        try
        {
            Lancamento lancamento = await _repository.GetEntityById(id);
            await _repository.Delete(lancamento);
            return Ok(true);
        }
        catch (Exception)
        {
            return BadRequest(false);
        }
    }
}
