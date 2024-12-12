using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

/// <summary>���~����</summary>
public class MainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MainExceptionHandler> logger;

    public MainExceptionHandler(ILogger<MainExceptionHandler> logger) => this.logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(exception, "[Http Error] ���楢��");
        // Return false to continue with the default behavior
        // - or - return true to signal that this exception is handled

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;

        var model = new { code = 999, message = "�եιL�{�o�ͤ������~" };
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(model));
        return true;
    }
}
