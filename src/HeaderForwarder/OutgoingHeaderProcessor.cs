using HeaderForwarder.internals;
using Microsoft.Extensions.DependencyInjection;

namespace HeaderForwarder
{
    /// <summary>
    /// Processor to process headers attached to outgoing request.
    /// </summary>
    public sealed class OutgoingHeaderProcessor : IOutgoingHeaderProcessor
    {
        private static IOutgoingHeaderProcessor? _processor;

        /// <summary>
        /// Adds the headers to outgoing request.
        /// </summary>
        /// <param name="setup">The delegate to process headers attached to outgoing request.</param>
        /// <param name="clear">Indicating whether to clear the existing headers in advanced.</param>
        /// <returns>
        /// The scope in which the added headers take effect.
        /// </returns>
        public IDisposable AddHeaders(Action<HeaderDictionary> setup, bool clear)
        {
            Guard.ArgumentNotNull(setup, nameof(setup));
            var scope = new HttpClientInvocationContextScope(clear);
            setup(HttpClientInvocationContext.Current!.OutgoingHeaders);
            return scope;
        }

        /// <summary>
        /// Suppresses the header forwarder from forwarding headers.
        /// </summary>
        /// <returns>
        /// The header forwarding suppress scope.
        /// </returns>
        public IDisposable SuppressHeaderForwarder() => new HeaderForwarderSuppressContextScope(true);

        /// <summary>
        /// Creates a new <see cref="IOutgoingHeaderProcessor"/> instance.
        /// </summary>
        /// <param name="headerNames">The names of headers to be forwarded.</param>
        /// <returns>The created <see cref="IOutgoingHeaderProcessor"/> instance.</returns>
        public static IOutgoingHeaderProcessor Create(params string[] headerNames)
        {
            if (_processor is not null) return _processor;
            var services = new ServiceCollection().AddHeaderForwarder(headerNames).BuildServiceProvider();
            HttpClientObserver.TryRegister(services.GetRequiredService<HttpClientObserver>());
            return _processor = services.GetRequiredService<IOutgoingHeaderProcessor>();
        }
    }
}
