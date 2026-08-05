using Barbearia.Application.Excecoes;
using Microsoft.AspNetCore.Mvc;

namespace BarbeariaApi.Middlewares;

public sealed class TratamentoExcecoesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TratamentoExcecoesMiddleware> _logger;

    public TratamentoExcecoesMiddleware(
        RequestDelegate next,
        ILogger<TratamentoExcecoesMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NaoEncontradoException exception)
        {
            await EscreverProblemDetailsAsync(
                context,
                StatusCodes.Status404NotFound,
                "Recurso não encontrado.",
                exception.Message);
        }
        catch (ConflitoException exception)
        {
            await EscreverProblemDetailsAsync(
                context,
                StatusCodes.Status409Conflict,
                "Conflito de dados.",
                exception.Message);
        }
        catch (ArgumentException exception)
        {
            await EscreverProblemDetailsAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Requisição inválida.",
                exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Erro não tratado ao processar a requisição {Metodo} {Caminho}.",
                context.Request.Method,
                context.Request.Path);

            await EscreverProblemDetailsAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Erro interno do servidor.",
                "Ocorreu um erro inesperado ao processar a solicitação.");
        }
    }

    private static async Task EscreverProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string titulo,
        string detalhe)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = titulo,
            Detail = detalhe,
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            context.TraceIdentifier;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}