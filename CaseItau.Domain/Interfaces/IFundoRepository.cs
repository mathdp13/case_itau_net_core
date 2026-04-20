using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface IFundoRepository
{
    Task<IEnumerable<Fundo>> GetAllAsync();
    Task<Fundo?> GetByCodigoAsync(string codigo);
    Task AddAsync(Fundo fundo);
    void Update(Fundo fundo);
    void Delete(Fundo fundo);
}
