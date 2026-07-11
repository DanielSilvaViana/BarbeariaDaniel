using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Domain.Entidades;

public class Barbeiro
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string? Especialidade { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Barbeiro()
    {
        Nome = string.Empty;
    }

    public Barbeiro(string nome, string? especialidade = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do barbeiro é obrigatório.");

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Especialidade = especialidade?.Trim();
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(string nome, string? especialidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do barbeiro é obrigatório.");

        Nome = nome.Trim();
        Especialidade = especialidade?.Trim();
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }
}
