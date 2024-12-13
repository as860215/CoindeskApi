using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

/// <summary>錯誤捕獲</summary>
public class MainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MainExceptionHandler> logger;
    private const string SystemErrorCode = "S999";
    private const string BusinessErrorCode = "B007";

    public MainExceptionHandler(ILogger<MainExceptionHandler> logger) => this.logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ResponseBodyModel model;
        // 如果是 BusinessException 的話，將錯誤訊息往外拋
        // 否則 將錯誤訊息隱藏並記錄Log
        if(exception is BusinessException)
        {
            model = new ResponseBodyModel(BusinessErrorCode, exception.Message);
        }
        else
        {
            const string errorMessage = "調用過程發生內部錯誤。";
            logger.LogError(exception, "[Http Error] 執行失敗");
            model = new ResponseBodyModel(SystemErrorCode, errorMessage);
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(model));
        return true;
    }

    /// <summary>錯誤回傳用模型</summary>
    /// <param name="Code">代號</param>
    /// <param name="Message">訊息</param>
    private record ResponseBodyModel(string Code, string Message);
}
