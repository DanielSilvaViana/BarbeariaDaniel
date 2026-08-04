using Barbearia.Application.DTOs.Clientes;
using FluentValidation;

namespace Barbearia.Application.Validacoes.Clientes;

public sealed class CadastrarClienteRequestValidator
    : AbstractValidator<CadastrarClienteRequest>
{
    public CadastrarClienteRequestValidator()
    {
        RuleFor(request => request.Nome)
            .NotEmpty()
            .WithMessage("O nome do cliente é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve possuir pelo menos 3 caracteres.")
            .MaximumLength(150)
            .WithMessage("O nome deve possuir no máximo 150 caracteres.");

        RuleFor(request => request.Telefone)
            .NotEmpty()
            .WithMessage("O telefone do cliente é obrigatório.")
            .MinimumLength(8)
            .WithMessage("O telefone deve possuir pelo menos 8 caracteres.")
            .MaximumLength(20)
            .WithMessage("O telefone deve possuir no máximo 20 caracteres.");

        RuleFor(request => request.Email)
            .EmailAddress()
            .WithMessage("O e-mail informado é inválido.")
            .MaximumLength(150)
            .WithMessage("O e-mail deve possuir no máximo 150 caracteres.")
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}