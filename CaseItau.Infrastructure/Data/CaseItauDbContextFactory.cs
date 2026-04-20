using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CaseItau.Infrastructure.Data;

// Used only at design time (dotnet ef migrations). Not called at runtime.
public class CaseItauDbContextFactory : IDesignTimeDbContextFactory<CaseItauDbContext>
{
    public CaseItauDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CaseItauDbContext>()
            .UseSqlite("Data Source=caseItau.db")
            .Options;

        return new CaseItauDbContext(options);
    }
}
