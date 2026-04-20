namespace CaseItau.Domain.Entities;

public class TipoFundo
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Fundo> Fundos { get; set; } = new List<Fundo>();
}
