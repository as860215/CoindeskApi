using Microsoft.AspNetCore.Mvc.Filters;
using Utility.Security;

/// <summary>驗證過濾器</summary>
public class AuthorizationFilter : IAsyncActionFilter
{
    /// <summary>認證鍵值</summary>
    private const string authKey = "as860215-key";

    private JwtAuth? _jwtAuth = null;
    private JwtAuth jwtAuth => LazyInitializer.EnsureInitialized(ref  _jwtAuth, () => new JwtAuth(authKey));

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 測試用的token
        // eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6ImFzODYwMjE1In0.M0tRVQ6B8eKH5VY23AXalXQqaBi9yKMPape4pX9L-EW0V56x72O3T1DrQfsKjWhC0o8uIBumz9uQM4tb90dSiQ

        var authorization = context.HttpContext.Request.Headers.Authorization.ToString();
        const string bearerTokenTitle = "Bearer ";
        const string invalidMessage = "身分驗證失敗";

        if (authorization.StartsWith(bearerTokenTitle))
        {
            var token = authorization.Substring(bearerTokenTitle.Length).Trim();
            if(!jwtAuth.AuthorizationToken(token))
                throw new BusinessException(invalidMessage);
        }
        else
            throw new BusinessException(invalidMessage);

        await next();
    }
}
