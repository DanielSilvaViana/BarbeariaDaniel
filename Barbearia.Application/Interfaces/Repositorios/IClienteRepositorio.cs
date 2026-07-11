using System;
using System.Collections.Generic;
using System.Text;

using Barbearia.Domain.Entidades;

namespace Barbearia.Application.Interfaces.Repositorios;

public interface IClienteRepositorio
{
    Task AdicionarAsync(
        Cliente cliente,
        CancellationToken cancellationToken);

    Task<Cliente?> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Cliente>> ListarAsync(
        CancellationToken cancellationToken);

    Task<bool> ExisteTelefoneAsync(
        string telefone,
        CancellationToken cancellationToken);

    Task SalvarAlteracoesAsync(
        CancellationToken cancellationToken);
}
