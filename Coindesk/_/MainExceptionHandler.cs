using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

/// <summary>���~����</summary>
public class MainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MainExceptionHandler> logger;
    private const string SystemErrorCode = "S999";
    private const string BusinessErrorCode = "B007";

    public MainExceptionHandler(ILogger<MainExceptionHandler> logger) => this.logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ResponseBodyModel model;
        // �p�G�O BusinessException ���ܡA�N���~�T�����~��
        // �_�h �N���~�T�����èðO��Log
        if(exception is BusinessException)
        {
            model = new ResponseBodyModel(BusinessErrorCode, exception.Message);
        }
        else
        {
            const string errorMessage = "�եιL�{�o�ͤ������~�C";
            logger.LogError(exception, "[Http Error] ���楢��");
            model = new ResponseBodyModel(SystemErrorCode, errorMessage);
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(model));
        return true;
    }

    /// <summary>���~�^�ǥμҫ�</summary>
    /// <param name="Code">�N��</param>
    /// <param name="Message">�T��</param>
    private record ResponseBodyModel(string Code, string Message);
}
