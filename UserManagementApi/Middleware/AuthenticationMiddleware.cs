namespace UserManagementApi.Middleware;

public class AuthenticationMiddleware
{
    private RequestDelegate _next;
    private ILogger<AuthenticationMiddleware> _logger;
    private const string DummyValidToken = "secret-token-123";

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Check if the Authorization header exists
        if (!context.Request.Headers.TryGetValue("Authorization", out var extractedToken))
        {
            _logger.LogWarning("Authorization header missing.");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized: Token is missing.");
            return;
        }

        // 2. Validate the token (Simple string check for this exercise)
        if (extractedToken != DummyValidToken)
        {
            _logger.LogWarning("Invalid token provided: {Token}", extractedToken);
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized: Invalid token.");
            return;
        }

        // 3. Token is valid! Move to the next middleware (or Controller)
        await _next(context);
    }
}