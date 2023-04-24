using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
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


}
