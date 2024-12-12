using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

/// <summary>錯誤捕獲</summary>
public class MainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MainExceptionHandler> logger;

    public MainExceptionHandler(ILogger<MainExceptionHandler> logger) => this.logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(exception, "[Http Error] 執行失敗");
        // Return false to continue with the default behavior
        // - or - return true to signal that this exception is handled

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;

        var model = new { code = 999, message = "調用過程發生內部錯誤" };
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(model));
        return true;
    }
}
