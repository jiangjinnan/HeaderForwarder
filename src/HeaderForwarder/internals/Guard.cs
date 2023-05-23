using System.Runtime.CompilerServices;

namespace HeaderForwarder
{
    internal static class Guard
    {
        public static T ArgumentNotNull<T>(T value, [CallerArgumentExpression(nameof(value))] string name = "") where T : class
        {
            return value ?? throw new ArgumentNullException(name);
        }

        public static IEnumerable<T> ArgumentNotNullOrEmpty<T>(IEnumerable<T> value, [CallerArgumentExpression(nameof(value))] string name = "")
        {
            ArgumentNotNull(value, name);
            if (!value.Any())
            {
                throw new ArgumentException("Argument value cannot be an empty collection", name);
            }

            return value;
        }

        public static string ArgumentNotNullOrWhiteSpace(string value, [CallerArgumentExpression(nameof(value))] string name = "")
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Argument value cannot be null or a white space string.", name);
            }

            return value;
        }
    }
}
