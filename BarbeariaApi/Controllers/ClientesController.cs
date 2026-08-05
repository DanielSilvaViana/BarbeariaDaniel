using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BarbeariaApi.Controllers;

[ApiController]
[Route("api/clientes")]
public sealed class ClientesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(
        typeof(ClienteResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CadastrarAsync(
        [FromBody] CadastrarClienteRequest request,
        [FromServices] ICadastrarClienteCasoDeUso casoDeUso,
        [FromServices] IValidator<CadastrarClienteRequest> validator,
        CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(
            request,
            cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .GroupBy(erro => erro.PropertyName)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo
                        .Select(erro => erro.ErrorMessage)
                        .ToArray());

            return BadRequest(
                new ValidationProblemDetails(erros)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Ocorreram erros de validação.",
                    Detail = "Verifique os campos informados."
                });
        }

        var cliente = await casoDeUso.ExecutarAsync(
            request,
            cancellationToken);

        return CreatedAtRoute(
            "ConsultarClientePorId",
            new { id = cliente.Id },
            cliente);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<ClienteResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAsync(
        [FromServices] IListarClientesCasoDeUso casoDeUso,
        CancellationToken cancellationToken)
    {
        var clientes = await casoDeUso.ExecutarAsync(
            cancellationToken);

        return Ok(clientes);
    }

    [HttpGet(
        "{id:guid}",
        Name = "ConsultarClientePorId")]
    [ProducesResponseType(
        typeof(ClienteResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConsultarPorIdAsync(
        Guid id,
        [FromServices] IConsultarClienteCasoDeUso casoDeUso,
        CancellationToken cancellationToken)
    {
        var cliente = await casoDeUso.ExecutarAsync(
            id,
            cancellationToken);

        return Ok(cliente);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(ClienteResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AtualizarAsync(
        Guid id,
        [FromBody] AtualizarClienteRequest request,
        [FromServices] IAtualizarClienteCasoDeUso casoDeUso,
        [FromServices] IValidator<AtualizarClienteRequest> validator,
        CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(
            request,
            cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .GroupBy(erro => erro.PropertyName)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo
                        .Select(erro => erro.ErrorMessage)
                        .ToArray());

            return BadRequest(
                new ValidationProblemDetails(erros)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Ocorreram erros de validação.",
                    Detail = "Verifique os campos informados."
                });
        }

        var cliente = await casoDeUso.ExecutarAsync(
            id,
            request,
            cancellationToken);

        return Ok(cliente);
    }
}