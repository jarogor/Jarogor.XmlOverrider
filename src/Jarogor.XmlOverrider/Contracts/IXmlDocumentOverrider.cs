﻿using System.Xml;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Overrides for xml string
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IXmlDocumentOverrider<out T> {
    /// <summary>
    ///     Adding an override xml
    /// </summary>
    /// <param name="overridingXmlDocument"></param>
    /// <returns></returns>
    public T AddOverride(XmlDocument overridingXmlDocument);

    /// <summary>
    ///     Adding an override xml string
    /// </summary>
    /// <param name="overridingXml"></param>
    /// <returns></returns>
    public T AddOverride(string overridingXml);
}
