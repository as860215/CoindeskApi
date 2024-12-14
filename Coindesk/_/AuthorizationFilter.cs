using Coindesk;
using Microsoft.AspNetCore.Mvc.Filters;
using Utility.Security;

/// <summary>���ҹL�o��</summary>
public class AuthorizationFilter : IAsyncActionFilter
{
    private JwtAuth? _jwtAuth = null;
    private JwtAuth jwtAuth => LazyInitializer.EnsureInitialized(ref  _jwtAuth, () => new JwtAuth(SecurityDefine.AuthKey));

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // ���եΪ�token
        // eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6ImFzODYwMjE1In0.M0tRVQ6B8eKH5VY23AXalXQqaBi9yKMPape4pX9L-EW0V56x72O3T1DrQfsKjWhC0o8uIBumz9uQM4tb90dSiQ

        // ����Token�����}���ݭn����
        if (context.HttpContext.Request.Path.Value?.EndsWith("Token/GenerateToken") == false)
        {
            var authorization = context.HttpContext.Request.Headers.Authorization.ToString();
            const string bearerTokenTitle = "Bearer ";
            const string invalidMessage = "�������ҥ���";
            var token = authorization;

            if (authorization.StartsWith(bearerTokenTitle))
                token = authorization.Substring(bearerTokenTitle.Length).Trim();

            if (!jwtAuth.AuthorizationToken(token))
                throw new BusinessException(invalidMessage);

        }

        await next();
    }
}
