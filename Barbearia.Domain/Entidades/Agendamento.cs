using System;
using System.Collections.Generic;
using System.Text;

using Barbearia.Domain.Enumeradores;

namespace Barbearia.Domain.Entidades;

public class Agendamento
{
    public Guid Id { get; private set; }

    public Guid ClienteId { get; private set; }
    public Guid BarbeiroId { get; private set; }
    public Guid ServicoId { get; private set; }

    public DateTime DataHora { get; private set; }
    public StatusAgendamento Status { get; private set; }
    public string? Observacao { get; private set; }

    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    private Agendamento()
    {
    }

    public Agendamento(
        Guid clienteId,
        Guid barbeiroId,
        Guid servicoId,
        DateTime dataHora,
        string? observacao = null)
    {
        if (clienteId == Guid.Empty)
            throw new ArgumentException("O cliente é obrigatório.");

        if (barbeiroId == Guid.Empty)
            throw new ArgumentException("O barbeiro é obrigatório.");

        if (servicoId == Guid.Empty)
            throw new ArgumentException("O serviço é obrigatório.");

        if (dataHora <= DateTime.UtcNow)
            throw new ArgumentException(
                "A data do agendamento deve ser futura.");

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        BarbeiroId = barbeiroId;
        ServicoId = servicoId;
        DataHora = dataHora;
        Observacao = observacao?.Trim();
        Status = StatusAgendamento.Agendado;
        CriadoEm = DateTime.UtcNow;
    }

    public void Confirmar()
    {
        if (Status != StatusAgendamento.Agendado)
            throw new InvalidOperationException(
                "Somente agendamentos pendentes podem ser confirmados.");

        Status = StatusAgendamento.Confirmado;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void IniciarAtendimento()
    {
        if (Status != StatusAgendamento.Confirmado)
            throw new InvalidOperationException(
                "O agendamento precisa estar confirmado.");

        Status = StatusAgendamento.EmAtendimento;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Finalizar()
    {
        if (Status != StatusAgendamento.EmAtendimento)
            throw new InvalidOperationException(
                "O atendimento precisa estar iniciado.");

        Status = StatusAgendamento.Finalizado;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status is StatusAgendamento.Finalizado
            or StatusAgendamento.Cancelado)
        {
            throw new InvalidOperationException(
                "Não é possível cancelar este agendamento.");
        }

        Status = StatusAgendamento.Cancelado;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Reagendar(DateTime novaDataHora)
    {
        if (Status is StatusAgendamento.Finalizado
            or StatusAgendamento.Cancelado)
        {
            throw new InvalidOperationException(
                "Não é possível reagendar este agendamento.");
        }

        if (novaDataHora <= DateTime.UtcNow)
            throw new ArgumentException(
                "A nova data deve ser futura.");

        DataHora = novaDataHora;
        AtualizadoEm = DateTime.UtcNow;
    }
}
