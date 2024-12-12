using System.Text;

/// <summary>��x���������h</summary>
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
        // �����ШD�i��
        logger.LogInformation(await ProcessRequest(context.Request));

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // �I�s�U�@�� Middleware
        await next(context);

        // �����^���X�h
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
