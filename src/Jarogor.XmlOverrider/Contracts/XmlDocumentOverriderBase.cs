using System.Xml;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Base overriding for xml
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class XmlDocumentOverriderBase<T> : OverriderBase<T>, IXmlDocumentOverrider<T> {
    /// <inheritdoc />
    protected XmlDocumentOverriderBase(Rules rules, ILogger<T> logger) : base(rules, logger) { }

    /// <inheritdoc />
    public abstract T AddOverride(XmlDocument overridingXmlDocument);

    /// <inheritdoc />
    public abstract T AddOverride(string overridingXml);
}
