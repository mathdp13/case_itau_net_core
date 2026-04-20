using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Repositories;

public class TipoFundoRepository : ITipoFundoRepository
{
    private readonly CaseItauDbContext _context;

    public TipoFundoRepository(CaseItauDbContext context) => _context = context;

    public async Task<IEnumerable<TipoFundo>> GetAllAsync()
        => await _context.TiposFundo.ToListAsync();

    public async Task<TipoFundo?> GetByCodigoAsync(int codigo)
        => await _context.TiposFundo.FindAsync(codigo);
}
