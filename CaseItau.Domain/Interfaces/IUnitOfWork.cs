namespace CaseItau.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFundoRepository Fundos { get; }
    ITipoFundoRepository TiposFundo { get; }
    Task<int> CommitAsync();
}
