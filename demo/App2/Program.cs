using HeaderForwarder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHeaderForwarder("foo", "bar").AddHttpClient();
var app = builder.Build();
app.MapGet("/test", async (HttpRequest request, IHttpClientFactory httpClientFactory, IOutgoingHeaderProcessor processor) =>
{
    foreach (var kv in request.Headers)
    {
        Console.WriteLine($"{kv.Key}:{kv.Value}");
    }
    await httpClientFactory.CreateClient().GetAsync("http://localhost:5001/test");
});
app.Run("http://localhost:5000");