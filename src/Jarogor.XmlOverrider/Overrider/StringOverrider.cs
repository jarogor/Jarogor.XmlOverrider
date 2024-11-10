using System.Xml;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jarogor.XmlOverrider.Overrider;

/// <summary>
///     Overrides for xml string
/// </summary>
public sealed class StringOverrider : XmlDocumentOverrider {
    /// <inheritdoc />
    public StringOverrider(Rules rules, string xml, ILogger<StringOverrider>? logger = null)
        : base(rules, XmlToDocument(xml), logger ?? new NullLogger<StringOverrider>()) { }

    private static XmlDocument XmlToDocument(string xml) {
        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }
}
