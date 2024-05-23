using System.Xml;
using Microsoft.Extensions.Logging;
using XmlOverrider.Scheme;

namespace XmlOverrider.Overrider;

/// <summary>
/// Overrides for xml string
/// </summary>
public class StringOverrider : XmlDocumentOverrider
{
    /// <inheritdoc />
    public StringOverrider(Rules rules, string xml, ILogger<StringOverrider>? logger = null)
        : base(rules, XmlToDocument(xml), logger)
    {
    }

    private static XmlDocument XmlToDocument(string xml)
    {
        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }
}