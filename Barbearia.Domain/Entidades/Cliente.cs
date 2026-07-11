using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Domain.Entidades;

public class Cliente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Telefone { get; private set; }
    public string? Email { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Cliente()
    {
        Nome = string.Empty;
        Telefone = string.Empty;
    }

    public Cliente(string nome, string telefone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("O telefone do cliente é obrigatório.");

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Telefone = telefone.Trim();
        Email = email?.Trim();
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(string nome, string telefone, string? email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("O telefone do cliente é obrigatório.");

        Nome = nome.Trim();
        Telefone = telefone.Trim();
        Email = email?.Trim();
    }
}
