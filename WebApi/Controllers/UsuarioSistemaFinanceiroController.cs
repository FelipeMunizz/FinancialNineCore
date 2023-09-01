using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/")]
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

    [HttpPost("CadastrarUsuarioSistemaFinanceiro/{idSistema:int}")]
    [Produces("application/json")]
    public async Task<object> CadastrarUsuarioSistemaFinanceiro(int idSistema, string email)
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
        catch (Exception)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
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
        catch (Exception)
        {
            return NotFound(false);
        }
        return Ok(true);
    }
}
