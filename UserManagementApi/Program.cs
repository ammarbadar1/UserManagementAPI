using UserManagementApi.Middleware;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
    // Define a "fixed" policy
    options.AddFixedWindowLimiter(policyName: "fixed", opt =>
    {
        opt.PermitLimit = 10;           // Max 10 requests
        opt.Window = TimeSpan.FromSeconds(1000); // Per 10 seconds
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;             // Allow 2 requests to wait in line before dropping
    });

    // Optional: Custom response when limit is hit
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseRateLimiter();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<RequestCounterMiddleware>();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
