// Flight.HeaderForwarder.HeaderForwardObserver
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace HeaderForwarder.internals
{
    /// <summary>
    /// A custom Observer used to intercept <see cref="T:System.Net.Http.HttpClient" /> and attach headers to request message.
    /// </summary>
    internal sealed class HeaderForwardObserver : IObserver<KeyValuePair<string, object?>>
    {
        private static Func<object, HttpRequestMessage>? _requestAccessor;

        private readonly IOutgoingHeaderCollectionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Flight.HeaderForwarder.HeaderForwardObserver" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="T:System.ArgumentNullException">provider</exception>
        public HeaderForwardObserver(IOutgoingHeaderCollectionProvider provider) => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted() { }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error) { }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(KeyValuePair<string, object?> value)
        {
            if (value.Key != "System.Net.Http.HttpRequestOut.Start" || value.Value is null)
            {
                return;
            }
            if (!_provider.TryGetHeaders(out var headers))
            {
                return;
            }
            _requestAccessor ??= CreateRequestAccessor(value.Value.GetType());
            var outgoingHeaders = _requestAccessor(value.Value).Headers!;
            foreach (var kv in headers)
            {
                outgoingHeaders.Add(kv.Key, kv.Value.AsEnumerable());
            }
        }

        private static Func<object, HttpRequestMessage> CreateRequestAccessor(Type type)
        {
            var getRequestMethod = type.GetProperty("Request")!.GetMethod!;
            var payload = Expression.Parameter(typeof(object));
            var getRequest = Expression.Call(Expression.Convert(payload, type), getRequestMethod);
            return Expression.Lambda<Func<object, HttpRequestMessage>>(getRequest, payload).Compile();
        }
    }
}