using System.Collections.Generic;
using System.Xml;

using Microsoft.Extensions.Logging;

namespace XmlOverrider;

public class XmlDocumentOverrider : Overrider<XmlDocumentOverrider>, IStringOverrider<XmlDocumentOverrider>
{
    private readonly List<XmlDocument> _overridingXmlDocuments = new();

    /// <summary>
    /// With logger.
    /// </summary>
    /// <param name="logger">ILogger implementation</param>
    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="markupFilePath">Path to override markup file</param>
    /// <param name="schemeFilePath">Path to the override markup schema file</param>
    public XmlDocumentOverrider(
        ILogger? logger,
        XmlDocument xml,
        string markupFilePath,
        string? schemeFilePath = null
    ) : base(
        logger,
        markupFilePath,
        schemeFilePath
    )
    {
        TargetXml = xml;
    }

    /// <summary>
    /// Without logger (logger is null).
    /// </summary>
    /// <param name="xml">The xml file that needs to be overridden</param>
    /// <param name="markupFilePath">Path to override markup file</param>
    /// <param name="schemeFilePath">Path to the override markup schema file</param>
    public XmlDocumentOverrider(
        XmlDocument xml,
        string markupFilePath,
        string? schemeFilePath = null
    ) : base(
        markupFilePath,
        schemeFilePath
    )
    {
        TargetXml = xml;
    }

    protected sealed override XmlDocument TargetXml { get; set; }

    public override XmlDocumentOverrider Processing()
    {
        for (var index = 0; index < _overridingXmlDocuments.Count; index++)
        {
            Logger?.LogDebug("Processing {0}", index);
            Processing(_overridingXmlDocuments[index]);
        }

        return this;
    }

    public XmlDocumentOverrider AddOverride(XmlDocument overridingXmlDocument)
    {
        _overridingXmlDocuments.Add(overridingXmlDocument);
        return this;
    }
}