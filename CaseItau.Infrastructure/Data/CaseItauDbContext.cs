using CaseItau.Domain.Entities;
using CaseItau.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Data;

public class CaseItauDbContext : DbContext
{
    public CaseItauDbContext(DbContextOptions<CaseItauDbContext> options) : base(options) { }

    public DbSet<Fundo> Fundos => Set<Fundo>();
    public DbSet<TipoFundo> TiposFundo => Set<TipoFundo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FundoConfiguration());
        modelBuilder.ApplyConfiguration(new TipoFundoConfiguration());
    }
}
