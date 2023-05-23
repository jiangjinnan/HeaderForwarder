using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace HeaderForwarder.internals
{
    internal sealed class OutgoingHeaderCollectionProvider : IOutgoingHeaderCollectionProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISet<string> _headerNames;

        public OutgoingHeaderCollectionProvider(IHttpContextAccessor httpContextAccessor, IOptions<HeaderForwarderOptions> options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _headerNames = (options ?? throw new ArgumentNullException(nameof(options))).Value.HeaderNames;
        }

        public bool TryGetHeaders([NotNullWhen(true)] out IEnumerable<KeyValuePair<string, StringValues>>? headers)
        {
            HeaderDictionary? outgoingHeaders = null;

            if (!(HeaderForwarderSuppressContext.Current?.IsSuppressed ?? false))
            {
                var incomingHeaders = _httpContextAccessor.HttpContext?.Request?.Headers;
                if (incomingHeaders is not null)
                {
                    foreach (var kv in incomingHeaders)
                    {
                        if (_headerNames.Contains(kv.Key))
                        {
                            outgoingHeaders ??= new HeaderDictionary();
                            outgoingHeaders.Add(kv.Key, kv.Value);
                        }
                    }
                }
            }

            var invocationContext = HttpClientInvocationContext.Current;
            if (invocationContext is not null)
            {
                foreach (var kv in invocationContext.OutgoingHeaders)
                {
                    outgoingHeaders ??= new HeaderDictionary();
                    outgoingHeaders.Add(kv.Key, kv.Value);
                }
            }

            headers = outgoingHeaders;
            return headers is not null;
        }
    }
}
