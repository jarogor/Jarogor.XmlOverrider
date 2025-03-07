using System.Xml;

using Jarogor.XmlOverrider.Scheme;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jarogor.XmlOverrider.Overrider;

/// <summary>
///     Overrides for xml string
/// </summary>
public sealed class StringOverrider : XmlDocumentOverrider
{
    /// <inheritdoc />
    public StringOverrider(Rules rules, string xml, ILogger<StringOverrider>? logger = null)
        : base(rules, XmlToDocument(xml))
    {
        Logger.Log = logger ?? NullLogger<StringOverrider>.Instance;
    }

    private static XmlDocument XmlToDocument(string xml)
    {
        XmlDocument? document = new();
        document.LoadXml(xml);
        return document;
    }
}