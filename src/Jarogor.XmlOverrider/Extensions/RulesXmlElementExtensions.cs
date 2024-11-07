using System.Xml;

namespace Jarogor.XmlOverrider.Extensions;

/// <summary>
///     Extensions methods of XmlElement Rules
/// </summary>
public static class RulesXmlElementExtensions {
    private const string Name = "name";

    private const string TypeAttribute = "attribute";

    private const string AttributeIdKey = "attributeIdName";
    private const string AttributeIdValue = "attributeIdValue";

    private const string AttributeOverride = "override";
    private const string OverrideInnerXml = "innerXml";

    /// <summary>
    ///     Get element name
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetElementName(this XmlElement element)
        => element.GetAttribute(Name);

    /// <summary>
    ///     Check if a value can be overridden
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsOverridable(this XmlElement element)
        => element.HasAttribute(AttributeOverride);

    /// <summary>
    ///     Check if internal XML overrides
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsOverrideInnerXml(this XmlElement element)
        => element.Attributes[AttributeOverride]?.Value == OverrideInnerXml;

    /// <summary>
    ///     Check is attribute element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsAttributeElement(this XmlElement element)
        => element.LocalName == TypeAttribute;

    /// <summary>
    ///     Check is element type
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsElementType(this XmlElement element)
        => element.LocalName != TypeAttribute;

    /// <summary>
    ///     Is there an attribute id name?
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool HasAttributeIdName(this XmlElement element)
        => element.HasAttribute(AttributeIdKey);

    /// <summary>
    ///     Get attribute id name
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetAttributeIdName(this XmlElement element)
        => element.GetAttribute(AttributeIdKey);

    /// <summary>
    ///     Is there an attribute id value?
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool HasAttributeIdValue(this XmlElement element)
        => element.HasAttribute(AttributeIdValue);

    /// <summary>
    ///     Get attribute id value
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetAttributeIdValue(this XmlElement element)
        => element.GetAttribute(AttributeIdValue);
}
