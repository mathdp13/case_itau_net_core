using CaseItau.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/tipos-fundo")]
[Authorize]
public class TipoFundoController : ControllerBase
{
    private readonly ITipoFundoService _service;

    public TipoFundoController(ITipoFundoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> GetByCodigo(int codigo)
        => Ok(await _service.GetByCodigoAsync(codigo));
}
