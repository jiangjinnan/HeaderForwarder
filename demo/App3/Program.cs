using Microsoft.AspNetCore.Http;
using System.Net.Http;

var app = WebApplication.CreateBuilder(args).Build();
app.MapGet("/test",  (HttpRequest request) =>
{
    foreach (var kv in request.Headers)
    {
        Console.WriteLine($"{kv.Key}:{kv.Value}");
    }
});
app.Run("http://localhost:5001");