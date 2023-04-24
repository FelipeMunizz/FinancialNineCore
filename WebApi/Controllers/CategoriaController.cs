using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
}
