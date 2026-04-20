using CaseItau.Application.DTOs;

namespace CaseItau.Application.Interfaces;

public interface IFundoService
{
    Task<IEnumerable<FundoDto>> GetAllAsync();
    Task<FundoDto> GetByCodigoAsync(string codigo);
    Task<FundoDto> CreateAsync(CreateFundoDto dto);
    Task UpdateAsync(string codigo, UpdateFundoDto dto);
    Task DeleteAsync(string codigo);
    Task MovimentarPatrimonioAsync(string codigo, decimal valor);
}
