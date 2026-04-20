using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Repositories;

public class FundoRepository : IFundoRepository
{
    private readonly CaseItauDbContext _context;

    public FundoRepository(CaseItauDbContext context) => _context = context;

    public async Task<IEnumerable<Fundo>> GetAllAsync()
        => await _context.Fundos.Include(f => f.TipoFundo).ToListAsync();

    public async Task<Fundo?> GetByCodigoAsync(string codigo)
        => await _context.Fundos.Include(f => f.TipoFundo)
               .FirstOrDefaultAsync(f => f.Codigo == codigo);

    public async Task AddAsync(Fundo fundo)
        => await _context.Fundos.AddAsync(fundo);

    public void Update(Fundo fundo)
        => _context.Fundos.Update(fundo);

    public void Delete(Fundo fundo)
        => _context.Fundos.Remove(fundo);
}
