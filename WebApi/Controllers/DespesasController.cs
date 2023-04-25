﻿using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

//[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class DespesasController : ControllerBase
{
    private readonly InterfaceDespesa _repository;
    private readonly IDespesaServico _service;

    public DespesasController(InterfaceDespesa repository, IDespesaServico service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet("ListarDespesasUsuario")]
    [Produces("application/json")]
    public async Task<object> ListarDespesasUsuario(string email) => await _repository.ListaDespesasUsuario(email);

    [HttpGet("ObterDespesa/{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<Despesa>> ObterDespesa(int id)
    {
        Despesa despesa = await _repository.GetEntityById(id);
        return Ok(despesa);
    }

    [HttpGet("CarregarGraficos")]
    [Produces("application/json")]
    public async Task<object> CarregarGraficos(string email) => await _service.CarregaGraficos(email);

    [HttpPost("AdicionarDespesa")]
    [Produces("application/json")]
    public async Task<ActionResult<Despesa>> AdicionarDespesa(Despesa despesa)
    {
        await _service.AdicionarDespesa(despesa);
        return Ok(despesa);
    }

    [HttpPost("ImportarExtratoCSV/{idCategoria:int}")]
    public async Task<IActionResult> ImportarExtratoCSV(IFormFile arquivo, int idCategoria)
    {
        using var streamReader = new StreamReader(arquivo.OpenReadStream());
        await _service.ImportarDespeasExtratoCSV(streamReader, idCategoria);
        return Ok();
    }

    [HttpPut("AtualizarDespesa")]
    [Produces("application/json")]
    public async Task<ActionResult<Despesa>> AtualizarDespesa(Despesa despesa)
    {
        await _service.AtualizarDespesa(despesa);
        return Ok(despesa);
    }

    [HttpDelete("DeletarDespesa/{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarDespesa(int id)
    {
        try
        {
            Despesa despesa = await _repository.GetEntityById(id);
            await _repository.Delete(despesa);
            return Ok(true);
        }
        catch (Exception)
        {
            return BadRequest(false);
        }
    }
}