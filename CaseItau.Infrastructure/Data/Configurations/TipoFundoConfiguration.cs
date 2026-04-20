using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Data.Configurations;

public class TipoFundoConfiguration : IEntityTypeConfiguration<TipoFundo>
{
    public void Configure(EntityTypeBuilder<TipoFundo> builder)
    {
        builder.ToTable("TIPO_FUNDO");
        builder.HasKey(t => t.Codigo);
        builder.Property(t => t.Codigo).HasColumnName("CODIGO").IsRequired();
        builder.Property(t => t.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();

        builder.HasData(
            new TipoFundo { Codigo = 1, Nome = "Fundo de Renda Fixa" },
            new TipoFundo { Codigo = 2, Nome = "Fundo de Ações" },
            new TipoFundo { Codigo = 3, Nome = "Fundo Multimercado" },
            new TipoFundo { Codigo = 4, Nome = "Fundo de Câmbio" },
            new TipoFundo { Codigo = 5, Nome = "Fundo de Previdência" }
        );
    }
}
