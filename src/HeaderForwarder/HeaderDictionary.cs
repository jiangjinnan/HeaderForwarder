using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace HeaderForwarder
{
    /// <summary>
    /// Http header dictionary.
    /// </summary>
public sealed class HeaderDictionary : IEnumerable<KeyValuePair<string, StringValues>>
{
    private readonly Dictionary<string, List<string>> _headers = new();

    /// <summary>
    /// Adds a new header in the form of name/value pair.
    /// </summary>
    /// <param name="name">The name of header to be added.</param>
    /// <param name="value">The value of the header to be added.</param>
    /// <returns>The <see cref="HeaderDictionary"/> so that additional calls can be chained.</returns>
    public HeaderDictionary Add(string name, string value)
    {
        Guard.ArgumentNotNullOrWhiteSpace(name);
        var list = _headers.TryGetValue(name, out var v) ? v : _headers[name] = new List<string>();
        list.Add(value);
        return this;
    }

    /// <summary>
    /// Adds the a range of headers in the form of name/value pairs.
    /// </summary>
    /// <param name="headers">The headers to be added.</param>
    /// <returns>The <see cref="HeaderDictionary"/> so that additional calls can be chained.</returns>
    public HeaderDictionary AddRange(params (string Name, string Value)[] headers)
    {
        foreach (var ( k,v) in headers)
        { 
            Add(k, v);
        }
        return this;
    }

    /// <summary>
    /// Adds a new header or override an existing header.
    /// </summary>
    /// <value>
    /// A <see cref="StringValues"/> representing the value of header.
    /// </value>
    /// <param name="name">The name of the header.</param>
    /// <returns>A <see cref="StringValues"/> representing the value of header.</returns>
    public StringValues this[string name]
    {
        get
        {
            Guard.ArgumentNotNullOrWhiteSpace(name);
            if (!_headers.TryGetValue(name, out var list))
            { 
                throw new IndexOutOfRangeException(nameof(name));
            }
            return list.Count == 1 ? new StringValues(list[0]) : new StringValues(list.ToArray());
        }
        set
        {
            (_headers[name] = new List<string>()).Add(value);
        }
    }

    /// <summary>
    /// Tries to remove an existing header.
    /// </summary>
    /// <param name="name">The name of the header to be removed.</param>
    /// <param name="value">The value of the header to be removed.</param>
    /// <returns>A <see cref="Boolean"/> value indicating if specified header is successfully removed.</returns>
    public bool TryRemove(string name, [NotNullWhen(true)]out StringValues? value)
    {
        Guard.ArgumentNotNullOrWhiteSpace(name);
        if (_headers.TryGetValue(name, out var list))
        {
            value = list.Count == 1 ? list[0] : new StringValues(list.ToArray());
            return true;
        }
        value = null;
        return false;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _headers.ToDictionary(it=>it.Key,it=> new StringValues(it.Value.ToArray())).GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
}
