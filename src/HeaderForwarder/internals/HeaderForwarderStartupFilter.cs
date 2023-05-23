using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace HeaderForwarder.internals
{
    internal class HeaderForwarderStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                HttpClientObserver.TryRegister(app.ApplicationServices.GetRequiredService<HttpClientObserver>());
                next(app);
            };
        }
    }
}
