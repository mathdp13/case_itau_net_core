using CaseItau.Application.DTOs;
using CaseItau.Application.Interfaces;
using CaseItau.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/fundos")]
[Authorize]
public class FundoController : ControllerBase
{
    private readonly IFundoService _service;
    private readonly IValidator<CreateFundoDto> _createValidator;
    private readonly IValidator<UpdateFundoDto> _updateValidator;

    public FundoController(
        IFundoService service,
        IValidator<CreateFundoDto> createValidator,
        IValidator<UpdateFundoDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
        => Ok(await _service.GetByCodigoAsync(codigo));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFundoDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCodigo), new { codigo = created.Codigo }, created);
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Update(string codigo, [FromBody] UpdateFundoDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        await _service.UpdateAsync(codigo, dto);
        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Delete(string codigo)
    {
        await _service.DeleteAsync(codigo);
        return NoContent();
    }

    [HttpPut("{codigo}/patrimonio")]
    public async Task<IActionResult> MovimentarPatrimonio(string codigo, [FromBody] MovimentarPatrimonioDto dto)
    {
        await _service.MovimentarPatrimonioAsync(codigo, dto.Valor);
        return NoContent();
    }
}
