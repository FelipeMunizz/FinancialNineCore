using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class SistemaFinanceiroController : ControllerBase
{
    private readonly InterfaceSistemaFinanceiro _repositorio;
    private readonly ISistemaFinanceiroServico _servico;

    public SistemaFinanceiroController(InterfaceSistemaFinanceiro repositorio, ISistemaFinanceiroServico servico)
    {
        _repositorio = repositorio;
        _servico = servico;
    }

    [HttpGet("ListaSistemasUsuario")]
    [Produces("application/json")]
    public async Task<object> ListaSistemaUsuario(string email)
    {
        return await _repositorio.ListaSistemasUsuario(email);
    }

    [HttpPost("AdicionarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<SistemaFinanceiro> AdicionarSistemaFinanceiro(SistemaFinanceiro sistema)
    {
        await _servico.AdicionarSistemaFinanceiro(sistema);

        return sistema;
    }

    [HttpPut("AtualizarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarSistemaFinanceiro(SistemaFinanceiro sistema)
    {
        await _servico.AtualizarSistemaFinanceiro(sistema);

        return Ok(sistema);
    }

    [HttpGet("ObterSistemaFinanceiro/{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<SistemaFinanceiro>> ObterSistemaFinanceiro(int id)
    {
        return await _repositorio.GetEntityById(id);       
    }

    [HttpDelete("DeletarSistemaFinanceiro/{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarSistemaFinanceiro(int id)
    {
        try
        {
            var system = await _repositorio.GetEntityById(id);

            await _repositorio.Delete(system);

            return Ok(true);
        }
        catch (Exception)
        {
            return BadRequest(false);
        }
    }
}
