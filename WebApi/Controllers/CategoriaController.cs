using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly InterfaceCategoria _repository;
    private readonly ICategoriaServico _service;

    public CategoriaController(InterfaceCategoria repository, ICategoriaServico service)
    {
        _repository = repository;
        _service = service;
    }
    
    [HttpGet("ObterCategoria/{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<Categoria>> ObterCategoria(int id)
    {
        Categoria categoria = await _repository.GetEntityById(id);
        return Ok(categoria);
    }

    [HttpGet("ListarCategoriasUsuario")]
    [Produces("application/json")]
    public async Task<object> ListarCategoriasUsuario(string email, int idSistema = 0)
    {
        return await _repository.ListarCategoriasUsuario(email, idSistema);
    }

    [HttpPost("AdicionarCategoria")]
    [Produces("application/json")]
    public async Task<object> AdicionarCategoria(Categoria categoria)
    {
        await _service.AdicionarCategoria(categoria);
        return Ok(categoria);
    }

    [HttpPut("AtualizarCategoria")]
    [Produces("application/json")]
    public async Task<object> AtualizarCategoria(Categoria categoria)
    {
        await _service.AutalizarCategoria(categoria);
        return Ok(categoria);
    }

    [HttpDelete("DeletarCategoria/{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarCategoria(int id)
    {
        try
        {
            Categoria categoria = await _repository.GetEntityById(id);
            await _repository.Delete(categoria);
            return Ok(true); ;
        }
        catch (Exception)
        {
            return BadRequest(false);
        }
    }
}
