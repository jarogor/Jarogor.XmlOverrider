using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using XmlOverrider.Contracts;

namespace XmlOverrider.Overrider;

/// <summary>
/// Overrides for xml
/// </summary>
public class XmlDocumentOverrider : OverriderBase<XmlDocumentOverrider>, IStringOverrider<XmlDocumentOverrider>
{
    private readonly List<XmlDocument> _overridingXmlDocuments = new();

    /// <inheritdoc />
    public XmlDocumentOverrider(
        ILogger<XmlDocumentOverrider> logger,
        XmlDocument xml,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        logger,
        rulesFilePath,
        schemeFilePath
    )
    {
        TargetXml = xml;
    }

    /// <inheritdoc />
    public XmlDocumentOverrider(
        ILogger<XmlDocumentOverrider> logger,
        XmlDocument xml,
        TextReader rulesStream,
        TextReader? schemeStream = null
    ) : base(
        logger,
        rulesStream,
        schemeStream
    )
    {
        TargetXml = xml;
    }

    /// <param name="xml">The xml that needs to be overridden</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
    public XmlDocumentOverrider(
        XmlDocument xml,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        new NullLogger<XmlDocumentOverrider>(),
        rulesFilePath,
        schemeFilePath
    )
    {
        TargetXml = xml;
    }

    /// <param name="xml">The xml file that needs to be overridden</param>
    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="schemeStream">Override rules schema stream</param>
    public XmlDocumentOverrider(
        XmlDocument xml,
        TextReader rulesStream,
        TextReader? schemeStream = null
    ) : base(
        new NullLogger<XmlDocumentOverrider>(),
        rulesStream,
        schemeStream
    )
    {
        TargetXml = xml;
    }

    /// <inheritdoc />
    protected sealed override XmlDocument TargetXml { get; set; }

    /// <inheritdoc />
    public override XmlDocumentOverrider Processing()
    {
        for (var index = 0; index < _overridingXmlDocuments.Count; index++)
        {
            Logger.LogDebug("Processing {0}", index);
            Processing(_overridingXmlDocuments[index]);
        }

        return this;
    }

    /// <inheritdoc />
    public XmlDocumentOverrider AddOverride(XmlDocument overridingXmlDocument)
    {
        _overridingXmlDocuments.Add(overridingXmlDocument);
        return this;
    }
}