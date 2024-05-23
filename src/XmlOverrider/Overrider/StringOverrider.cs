using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace XmlOverrider.Overrider;

/// <summary>
/// Overrides for xml string
/// </summary>
public class StringOverrider : XmlDocumentOverrider
{
    /// <inheritdoc />
    public StringOverrider(
        ILogger<StringOverrider> logger,
        string xml,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        logger,
        XmlToDocument(xml),
        rulesFilePath,
        schemeFilePath
    )
    {
    }

    /// <inheritdoc />
    public StringOverrider(
        ILogger<StringOverrider> logger,
        string xml,
        TextReader rulesStream,
        TextReader? schemeStream = null
    ) : base(
        logger,
        XmlToDocument(xml),
        rulesStream,
        schemeStream
    )
    {
    }

    /// <inheritdoc />
    public StringOverrider(
        string xml,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        new NullLogger<StringOverrider>(),
        XmlToDocument(xml),
        rulesFilePath,
        schemeFilePath
    )
    {
    }

    /// <inheritdoc />
    public StringOverrider(
        string xml,
        TextReader rulesStream,
        TextReader? schemeStream = null
    ) : base(
        new NullLogger<StringOverrider>(),
        XmlToDocument(xml),
        rulesStream,
        schemeStream
    )
    {
    }

    private static XmlDocument XmlToDocument(string xml)
    {
        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }
}