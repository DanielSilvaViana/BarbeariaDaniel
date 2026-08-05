using Barbearia.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Barbearia.Infrastructure.Dados.Configuracoes;

public sealed class ClienteConfiguracao
    : IEntityTypeConfiguration<Cliente>
{
    public void Configure(
        EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(cliente => cliente.Id);

        builder.Property(cliente => cliente.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(cliente => cliente.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(cliente => cliente.Telefone)
            .HasColumnName("telefone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(cliente => cliente.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(cliente => cliente.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.HasIndex(cliente => cliente.Telefone)
            .IsUnique()
            .HasDatabaseName("ux_clientes_telefone");
    }
}