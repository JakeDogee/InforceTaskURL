namespace InforceTask.WebApi.Middlewares;

using System.Net;
using System.Text.Json;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        AppResponse? response = null;

        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            response = new AppResponse().CreateWithOneMessage(ex);
        }
        catch (ForbiddenException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            response = new AppResponse().CreateWithOneMessage(ex);
        }
        catch (UnauthorizedException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            response = new AppResponse().CreateWithOneMessage(ex);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            response = new AppResponse().CreateWithOneMessage(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            response = new AppResponse().CreateWithOneMessage(ToUnhandledException(ex));
        }

        if (response != null)
        {
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static UnhandledException ToUnhandledException(Exception ex)
    {
        return new UnhandledException(new[]
        {
            ex.Message
        });
    }
}