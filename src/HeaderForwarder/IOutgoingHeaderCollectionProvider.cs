using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace HeaderForwarder
{
    /// <summary>
    /// Provides headers to be attached into outgoing request.
    /// </summary>
    public interface IOutgoingHeaderCollectionProvider
    {
        /// <summary>
        /// Tries to get headers to be attached to outgoing request.
        /// </summary>
        /// <param name="headers">The headers to be attached to outgoing request.</param>
        /// <returns>A <see cref="Boolean"/> value indicating if headers exist.</returns>
        bool TryGetHeaders([NotNullWhen(true)]out IEnumerable<KeyValuePair<string, StringValues>>? headers);
    }
}