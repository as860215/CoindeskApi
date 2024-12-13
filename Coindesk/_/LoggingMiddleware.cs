using System.Text;

/// <summary>日誌紀錄中介層</summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LoggingMiddleware> logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 紀錄請求進來
        using var requestBody = new MemoryStream();
        logger.LogInformation(await ProcessRequest(context.Request, requestBody));

        // 呼叫下一個 Middleware
        await next(context);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // 紀錄回應出去
        logger.LogInformation(await ProcessResponse(context.Response));
        await responseBody.CopyToAsync(originalBodyStream);
    }

    /// <summary>處理請求LOG</summary>
    /// <param name="request">Http Request</param>
    /// <param name="requestStream">Http Request Body用串流，用來將串流指標回到起點</param>
    private async Task<string> ProcessRequest(HttpRequest request, MemoryStream requestStream)
    {
        await request.Body.CopyToAsync(requestStream);
        requestStream.Position = 0;
        request.Body = requestStream;
        var text = Encoding.UTF8.GetString(requestStream.ToArray());

        return $"[Http Request][{request.Host}{request.Path}{request.QueryString}] {text}";
    }


    /// <summary>處理回應LOG</summary>
    /// <param name="response">Http Response</param>
    private async Task<string> ProcessResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"[Http Response] {text}";
    }
}
