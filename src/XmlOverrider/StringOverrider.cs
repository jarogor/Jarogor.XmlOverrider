using System.Xml;

using Microsoft.Extensions.Logging;

namespace XmlOverrider;

public class StringOverrider : XmlDocumentOverrider
{
    /// <summary>
    /// With logger.
    /// </summary>
    /// <param name="logger">ILogger implementation</param>
    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="markupFilePath">Path to override markup file</param>
    /// <param name="schemeFilePath">Path to the override markup schema file</param>
    public StringOverrider(
        ILogger? logger,
        string xml,
        string markupFilePath,
        string? schemeFilePath = null
    ) : base(
        logger,
        XmlToDocument(xml),
        markupFilePath,
        schemeFilePath
    )
    {
    }

    /// <summary>
    /// Without logger (logger is null).
    /// </summary>
    /// <param name="xml">The xml file that needs to be overridden</param>
    /// <param name="markupFilePath">Path to override markup file</param>
    /// <param name="schemeFilePath">Path to the override markup schema file</param>
    public StringOverrider(
        string xml,
        string markupFilePath,
        string? schemeFilePath = null
    ) : base(
        XmlToDocument(xml),
        markupFilePath,
        schemeFilePath
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