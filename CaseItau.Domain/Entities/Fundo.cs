namespace CaseItau.Domain.Entities;

public class Fundo
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public int CodigoTipo { get; set; }
    public decimal? Patrimonio { get; set; }

    public TipoFundo TipoFundo { get; set; } = null!;
}
