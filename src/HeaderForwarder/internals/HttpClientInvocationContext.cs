namespace HeaderForwarder.internals
{
    internal sealed class HttpClientInvocationContext
    {
        private HeaderDictionary? _headers;
        private static AsyncLocal<HttpClientInvocationContext?> _currentAccessor = new();

        /// <summary>
        /// Gets the current <see cref="T:Flight.HeaderForwarder.HttpInvocationContext" />.
        /// </summary>
        /// <value>
        /// The current <see cref="T:Flight.HeaderForwarder.HttpInvocationContext" />.
        /// </value>
        public static HttpClientInvocationContext? Current
        {
            get => _currentAccessor.Value;
            set => _currentAccessor.Value = value;
        }

        /// <summary>
        /// Gets the outgoing headers.
        /// </summary>
        /// <value>
        /// The outgoing headers.
        /// </value>
        public HeaderDictionary OutgoingHeaders => _headers ??= new HeaderDictionary();
    }

    internal sealed class HttpClientInvocationContextScope : IDisposable
    {
        private readonly HttpClientInvocationContext? _original = HttpClientInvocationContext.Current;
        public HttpClientInvocationContextScope(bool suppressParent = false)
        {
            if (_original is null || suppressParent)
            {
                HttpClientInvocationContext.Current = new HttpClientInvocationContext();
            }
        }
        public void Dispose() => HttpClientInvocationContext.Current = _original;
    }
}
