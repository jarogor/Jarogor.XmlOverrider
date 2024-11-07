using System.Collections.Generic;
using System.Xml;
using Jarogor.XmlOverrider.Contracts;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider.Overrider;

/// <summary>
///     Overrides for xml
/// </summary>
public class XmlDocumentOverrider : OverriderBase<XmlDocumentOverrider>, IStringOverrider<XmlDocumentOverrider> {
    private readonly List<XmlDocument> _overridingXmlDocuments = new();

    /// <inheritdoc />
    public XmlDocumentOverrider(Rules rules, XmlDocument xml, ILogger<XmlDocumentOverrider>? logger = null)
        : base(rules, logger) {
        TargetXml = xml;
    }

    /// <inheritdoc />
    protected sealed override XmlDocument TargetXml { get; set; }

    /// <inheritdoc />
    public XmlDocumentOverrider AddOverride(XmlDocument overridingXmlDocument) {
        _overridingXmlDocuments.Add(overridingXmlDocument);
        return this;
    }

    /// <inheritdoc />
    public override XmlDocumentOverrider Processing() {
        for (var index = 0; index < _overridingXmlDocuments.Count; index++) {
            Logger.LogDebug("Processing {0}", index);
            Processing(_overridingXmlDocuments[index]);
        }

        return this;
    }
}
