namespace HeaderForwarder
{
    /// <summary>
    /// Header forwarder options.
    /// </summary>
    public sealed class HeaderForwarderOptions
    {
        /// <summary>
        /// Gets the names of the headers to be forwarded.
        /// </summary>
        /// <value>
        /// The names of the headers to be forwarded.
        /// </value>
        public ISet<string> HeaderNames { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
