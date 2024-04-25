using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace XmlOverrider.Overrider;

public class StringOverrider : XmlDocumentOverrider
{
    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
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

    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="schemeStream">Override rules schema stream</param>
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

    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
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

    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="schemeStream">Override rules schema stream</param>
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