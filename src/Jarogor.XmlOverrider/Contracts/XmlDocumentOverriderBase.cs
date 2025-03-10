﻿using System.Xml;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Base overriding for xml
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class XmlDocumentOverriderBase<T> : OverriderBase<T>, IXmlDocumentOverrider<T>
{
    /// <inheritdoc />
    protected XmlDocumentOverriderBase(OverrideRules[] rules) : base(rules) { }

    /// <inheritdoc />
    public abstract T AddOverride(XmlDocument overridingXmlDocument);

    /// <inheritdoc />
    public abstract T AddOverride(string overridingXml);
}