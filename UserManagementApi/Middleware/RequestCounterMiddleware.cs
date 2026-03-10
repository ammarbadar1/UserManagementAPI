namespace UserManagementApi.Middleware;

public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Dictionary<string, int> _requestCounts = new Dictionary<string, int>();

    public RequestCounterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var compositeKey = $"{method}_{path}";
        if (_requestCounts.ContainsKey(compositeKey))
        {
            _requestCounts[compositeKey]++;
        }
        else
        {
            _requestCounts[compositeKey] = 1;
        }

        context.Response.OnStarting(() =>
        {

            context.Response.Headers["X-Request-Count"] = _requestCounts[compositeKey].ToString();
            return Task.CompletedTask;
        });

        await _next(context);
    }
}