using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface ITipoFundoRepository
{
    Task<IEnumerable<TipoFundo>> GetAllAsync();
    Task<TipoFundo?> GetByCodigoAsync(int codigo);
}
