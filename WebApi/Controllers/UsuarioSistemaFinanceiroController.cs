using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class UsuarioSistemaFinanceiroController : ControllerBase
{
    private readonly IUsuarioSistemaFinanceiroServico _servico;
    private readonly InterfaceUsuarioSistemaFinanceiro _repositorio;

    public UsuarioSistemaFinanceiroController(IUsuarioSistemaFinanceiroServico servico, InterfaceUsuarioSistemaFinanceiro repositorio)
    {
        _servico = servico;
        _repositorio = repositorio;
    }

    [HttpGet("ListarUsuariosSistemaFinanceiro/{idSistema:int}")]
    [Produces("application/json")]
    public async Task<object> ListarUsuariosSistemaFinanceiro(int idSistema)
    {
        return await _repositorio.ListaUsuariosSistemasFinanceiro(idSistema);
    }

    [HttpPost("CadastrarSistemaFinanceiro/{idSistema:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> CadastrarUsuarioSistemaFinanceiro(int idSistema, [FromBody] string email)
    {
        try
        {
            await _servico.CadastraUsuarioSistema(
                new UsuarioSistemaFinanceiro
                {
                    IdSistema = idSistema,
                    EmailUsuario = email,
                    Administrador = false,
                    SistemaAtual = true
                });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok("Usuario Cadastrado com Sucesso");
    }

    [HttpDelete("DeletarUsuarioSistemaFinanceiro/{idUsuario:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarUsuarioSistemaFinanceiro(int idUsuario)
    {
        try
        {
            var userSystem = await _repositorio.GetEntityById(idUsuario);
            await _repositorio.Delete(userSystem);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
        return Ok("Usuario deletado com sucesso");
    }
}
