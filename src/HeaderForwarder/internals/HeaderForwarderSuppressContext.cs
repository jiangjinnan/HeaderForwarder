using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaderForwarder.internals
{
    internal sealed class HeaderForwarderSuppressContext
    {
        private static readonly AsyncLocal<HeaderForwarderSuppressContext?> _currentAccessor = new();
        public static HeaderForwarderSuppressContext? Current
        {
            get => _currentAccessor.Value;
            set => _currentAccessor.Value = value;
        }
        public bool IsSuppressed { get; }
        public HeaderForwarderSuppressContext(bool isSuppressed) => IsSuppressed = isSuppressed;
    }

    internal sealed class HeaderForwarderSuppressContextScope : IDisposable
    {
        private readonly HeaderForwarderSuppressContext? _original = HeaderForwarderSuppressContext.Current;
        public HeaderForwarderSuppressContextScope(bool isSuppressed) => HeaderForwarderSuppressContext.Current = new HeaderForwarderSuppressContext(isSuppressed);
        public void Dispose() => HeaderForwarderSuppressContext.Current = _original;
    }
}
