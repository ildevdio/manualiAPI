using ManualiAPI.Exceptions;

namespace ManualiAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex) when (
            ex is ProductNotFoundException or UsuarioNotFoundException
                or ArtesaoNotFoundException or AdmNotFoundException
                or PedidoNotFoundException)
        {
            await Responder(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (Exception ex) when (ex is UsuarioDuplicadoException or EstoqueInsuficienteException)
        {
            await Responder(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await Responder(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");
            await Responder(context, StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
        }
    }

    private static async Task Responder(HttpContext context, int statusCode, string mensagem)
    {
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { erro = mensagem });
    }
}