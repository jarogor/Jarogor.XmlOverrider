using System.Xml;

namespace XmlOverrider.Contracts;

/// <summary>
///     Overrides for xml string
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStringOverrider<out T> {
    /// <summary>
    ///     Adding an override xml
    /// </summary>
    /// <param name="overridingXmlDocument"></param>
    /// <returns></returns>
    public T AddOverride(XmlDocument overridingXmlDocument);
}
