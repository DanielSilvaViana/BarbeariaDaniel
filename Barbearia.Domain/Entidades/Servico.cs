using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Domain.Entidades;

public class Servico
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int DuracaoMinutos { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Servico()
    {
        Nome = string.Empty;
    }

    public Servico(
        string nome,
        decimal preco,
        int duracaoMinutos,
        string? descricao = null)
    {
        Validar(nome, preco, duracaoMinutos);

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Descricao = descricao?.Trim();
        Preco = preco;
        DuracaoMinutos = duracaoMinutos;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(
        string nome,
        decimal preco,
        int duracaoMinutos,
        string? descricao)
    {
        Validar(nome, preco, duracaoMinutos);

        Nome = nome.Trim();
        Descricao = descricao?.Trim();
        Preco = preco;
        DuracaoMinutos = duracaoMinutos;
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    private static void Validar(
        string nome,
        decimal preco,
        int duracaoMinutos)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do serviço é obrigatório.");

        if (preco <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");

        if (duracaoMinutos <= 0)
            throw new ArgumentException("A duração deve ser maior que zero.");
    }
}
