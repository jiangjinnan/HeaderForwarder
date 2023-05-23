using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace HeaderForwarder.internals
{
    internal sealed class HttpClientObserver : IObserver<DiagnosticListener>
    {
        private readonly IOutgoingHeaderCollectionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Flight.HeaderForwarder.HttpClientObserver" /> class.
        /// </summary>
        /// <param name="provider">The <see cref="T:Flight.HeaderForwarder.IOutgoingHeaderCollectionProvider" /> to get request headers to attach.</param>
        public HttpClientObserver(IOutgoingHeaderCollectionProvider provider) => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == "HttpHandlerDiagnosticListener")
            {
                value.Subscribe(new HeaderForwardObserver(_provider));
            }
        }

        internal static volatile bool IsRegistered = false;
        internal static void TryRegister(HttpClientObserver? httpClientObserver = null)
        {
            if (!IsRegistered)
            {
                httpClientObserver ??= new ServiceCollection().AddHeaderForwarder().BuildServiceProvider().GetRequiredService<HttpClientObserver>();
                DiagnosticListener.AllListeners.Subscribe(httpClientObserver);
                IsRegistered = true;
            }
        }
    }
}
