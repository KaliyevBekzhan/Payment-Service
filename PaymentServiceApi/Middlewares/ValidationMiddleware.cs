using System.Security.Cryptography;
using System.Text;
using PaymentServiceApi.Attributes;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private const string Secret = "my-dev-secret";

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint == null ||
            endpoint.Metadata.GetMetadata<RequireHmacAttribute>() == null)
        {
            await _next(context);
            return;
        }

        var signatureHeader = context.Request.Headers["X-Signature"].FirstOrDefault();

        if (string.IsNullOrEmpty(signatureHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing signature");
            return;
        }

        string stringToSign;

        if (context.Request.Method == HttpMethods.Get)
        {
            var pathAndQuery = context.Request.Path + context.Request.QueryString;
            stringToSign = context.Request.Method + pathAndQuery + Secret;
        }
        else
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            stringToSign = body + Secret;
        }

        var computed = ComputeHmac(stringToSign);

        if (!CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(signatureHeader),
            Encoding.UTF8.GetBytes(computed)))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid signature");
            return;
        }

        await _next(context);
    }

    private static string ComputeHmac(string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}