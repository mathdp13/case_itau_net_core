using CaseItau.Domain.Interfaces;
using CaseItau.Infrastructure.Data;
using CaseItau.Infrastructure.Repositories;

namespace CaseItau.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly CaseItauDbContext _context;

    public IFundoRepository Fundos { get; }
    public ITipoFundoRepository TiposFundo { get; }

    public UnitOfWork(CaseItauDbContext context)
    {
        _context = context;
        Fundos = new FundoRepository(context);
        TiposFundo = new TipoFundoRepository(context);
    }

    public Task<int> CommitAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
