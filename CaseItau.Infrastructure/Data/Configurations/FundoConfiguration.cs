using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Data.Configurations;

public class FundoConfiguration : IEntityTypeConfiguration<Fundo>
{
    public void Configure(EntityTypeBuilder<Fundo> builder)
    {
        builder.ToTable("FUNDO");
        builder.HasKey(f => f.Codigo);
        builder.Property(f => f.Codigo).HasColumnName("CODIGO").HasMaxLength(20).IsRequired();
        builder.Property(f => f.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
        builder.Property(f => f.Cnpj).HasColumnName("CNPJ").HasMaxLength(18).IsRequired();
        builder.Property(f => f.CodigoTipo).HasColumnName("CODIGO_TIPO").IsRequired();
        builder.Property(f => f.Patrimonio).HasColumnName("PATRIMONIO");

        builder.HasOne(f => f.TipoFundo)
               .WithMany(t => t.Fundos)
               .HasForeignKey(f => f.CodigoTipo);
    }
}
