using HeaderForwarder;
using HeaderForwarder.internals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines extension methods to added header forwarder's options.
    /// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the header forwarder.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing all service registrations.</param>
    /// <param name="headerNames">The names of the headers to be forwarded.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddHeaderForwarder(this IServiceCollection services, params string[] headerNames)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IOutgoingHeaderCollectionProvider, OutgoingHeaderCollectionProvider>();
            services.TryAddSingleton<IOutgoingHeaderProcessor,  OutgoingHeaderProcessor>();
            services.TryAddSingleton<HttpClientObserver>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, HeaderForwarderStartupFilter>());
            services.Configure<HeaderForwarderOptions>(options =>
            {
                foreach (var headerName in headerNames)
                {
                    options.HeaderNames.Add(headerName);
                }
            });
            return services;
        }
    }
}
