namespace HeaderForwarder
{
    /// <summary>
    /// Processor to process headers attached to outgoing request.
    /// </summary>
    public interface IOutgoingHeaderProcessor
    {
        /// <summary>
        /// Suppresses the header forwarder from forwarding headers.
        /// </summary>
        /// <returns>The header forwarding suppress scope.</returns>
        IDisposable SuppressHeaderForwarder();

        /// <summary>
        /// Adds the headers to outgoing request.
        /// </summary>
        /// <param name="setup">The delegate to process headers attached to outgoing request.</param>
        /// <param name="clear">Indicating whether to clear the existing headers in advanced.</param>
        /// <returns>The scope in which the added headers take effect.</returns>
        IDisposable AddHeaders(Action<HeaderDictionary> setup, bool clear);
    }
}
