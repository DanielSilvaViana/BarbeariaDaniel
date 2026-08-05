using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Domain.Entidades;
using Barbearia.Infrastructure.Dados.Contextos;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infrastructure.Repositorios;

public sealed class ClienteRepositorio : IClienteRepositorio
{
    private readonly BarbeariaDbContext _dbContext;

    public ClienteRepositorio(
        BarbeariaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(
        Cliente cliente,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cliente);

        await _dbContext.Clientes.AddAsync(
            cliente,
            cancellationToken);
    }

    public async Task<Cliente?> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Clientes
            .FirstOrDefaultAsync(
                cliente => cliente.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Cliente>> ListarAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Clientes
            .AsNoTracking()
            .OrderBy(cliente => cliente.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteTelefoneAsync(
        string telefone,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Clientes
            .AsNoTracking()
            .AnyAsync(
                cliente => cliente.Telefone == telefone,
                cancellationToken);
    }

    public async Task<bool> ExisteTelefoneParaOutroClienteAsync(
        string telefone,
        Guid clienteId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Clientes
            .AsNoTracking()
            .AnyAsync(
                cliente =>
                    cliente.Telefone == telefone &&
                    cliente.Id != clienteId,
                cancellationToken);
    }

    public async Task SalvarAlteracoesAsync(
        CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}