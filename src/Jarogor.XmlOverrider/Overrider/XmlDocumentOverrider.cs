using System.Collections.Generic;
using System.Xml;
using Jarogor.XmlOverrider.Contracts;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jarogor.XmlOverrider.Overrider;

/// <summary>
///     Overrides for xml
/// </summary>
public class XmlDocumentOverrider : XmlDocumentOverriderBase<XmlDocumentOverrider> {
    private readonly List<XmlDocument> _overridingXmlDocuments = new();

    /// <inheritdoc />
    public XmlDocumentOverrider(Rules rules, XmlDocument xml, ILogger<XmlDocumentOverrider>? logger = null)
        : base(rules, logger ?? new NullLogger<XmlDocumentOverrider>()) {
        TargetXml = xml;
    }

    /// <inheritdoc />
    protected sealed override XmlDocument TargetXml { get; set; }

    /// <inheritdoc />
    public override XmlDocumentOverrider AddOverride(XmlDocument overridingXmlDocument) {
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
