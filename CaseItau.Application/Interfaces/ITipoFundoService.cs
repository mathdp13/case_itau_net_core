using CaseItau.Application.DTOs;

namespace CaseItau.Application.Interfaces;

public interface ITipoFundoService
{
    Task<IEnumerable<TipoFundoDto>> GetAllAsync();
    Task<TipoFundoDto> GetByCodigoAsync(int codigo);
}
