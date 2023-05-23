using HeaderForwarder;

namespace HeaderForwarder
{
    /// <summary>
    /// Defines extension methods to process headers attached to outgoing request.
    /// </summary>
public static class OutgoingHeaderProcessorExtensions
{
    /// <summary>
    /// Adds the headers attached to to outgoing request.
    /// </summary>
    /// <param name="processor">The <see cref="IOutgoingHeaderProcessor"/>.</param>
    /// <param name="headers">The headers to be added.</param>
    /// <returns>The <see cref="IOutgoingHeaderProcessor"/> so that additional calls can be chained.</returns>
    public static IDisposable AddHeaders(this IOutgoingHeaderProcessor processor, params (string Key, string Value)[] headers)
    {
        Guard.ArgumentNotNull(processor);
        return processor.AddHeaders(requestHeaders => requestHeaders.AddRange(headers), false);
    }

    /// <summary>
    /// Add or replaces the headers attached to to outgoing request.
    /// </summary>
    /// <param name="processor">The <see cref="IOutgoingHeaderProcessor"/>.</param>
    /// <param name="headers">The headers to be added or replaced.</param>
    /// <returns>The <see cref="IOutgoingHeaderProcessor"/> so that additional calls can be chained.</returns>
    public static IDisposable ReplaceHeaders(this IOutgoingHeaderProcessor processor, params (string Key, string Value)[] headers)
    {
        Guard.ArgumentNotNull(processor);
        return processor.AddHeaders(requestHeaders => {
            foreach (var (key, value) in headers)
            {
                requestHeaders[key] = value;
            }
        }, false);
    }

    /// <summary>
    /// Adds the headers attached to to outgoing request after clearing the existing ones.
    /// </summary>
    /// <param name="processor">The <see cref="IOutgoingHeaderProcessor"/>.</param>
    /// <param name="headers">The headers to be added.</param>
    /// <returns>The <see cref="IOutgoingHeaderProcessor"/> so that additional calls can be chained.</returns>
    public static IDisposable AddHeadersAfterClear(this IOutgoingHeaderProcessor processor, params (string Key, string Value)[] headers)
    {
        Guard.ArgumentNotNull(processor);
        return processor.AddHeaders(requestHeaders => requestHeaders.AddRange(headers), true);
    }
}
}
