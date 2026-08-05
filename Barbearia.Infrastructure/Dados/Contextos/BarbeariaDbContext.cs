using Barbearia.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infrastructure.Dados.Contextos;

public sealed class BarbeariaDbContext : DbContext
{
    public BarbeariaDbContext(
        DbContextOptions<BarbeariaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BarbeariaDbContext).Assembly);
    }
}