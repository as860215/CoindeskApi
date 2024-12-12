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
        logger.LogInformation(await ProcessRequest(context.Request));

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // 呼叫下一個 Middleware
        await next(context);

        // 紀錄回應出去
        logger.LogInformation(await ProcessResponse(context.Response));
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private async Task<string> ProcessRequest(HttpRequest request)
    {
        var body = request.Body;

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        var text = Encoding.UTF8.GetString(buffer);
        request.Body = body;

        return $"[Http Request][{request.Host}{request.Path}{request.QueryString}] {text}";
    }

    private async Task<string> ProcessResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"[Http Response] {text}";
    }
}
