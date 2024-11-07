using System.Xml;

namespace Jarogor.XmlOverrider.Extensions;

public static class RulesXmlElementExtensions {
    private const string Name = "name";

    private const string TypeAttribute = "attribute";

    private const string AttributeIdKey = "attributeIdName";
    private const string AttributeIdValue = "attributeIdValue";

    private const string AttributeOverride = "override";
    private const string OverrideInnerXml = "innerXml";

    public static string GetElementName(this XmlElement element)
        => element.GetAttribute(Name);

    public static bool IsOverridable(this XmlElement element)
        => element.HasAttribute(AttributeOverride);

    public static bool IsOverrideInnerXml(this XmlElement element)
        => element.Attributes[AttributeOverride]?.Value == OverrideInnerXml;

    public static bool IsAttributeElement(this XmlElement element)
        => element.LocalName == TypeAttribute;

    public static bool IsElementType(this XmlElement element)
        => element.LocalName != TypeAttribute;

    public static bool HasAttributeIdName(this XmlElement element)
        => element.HasAttribute(AttributeIdKey);

    public static string GetAttributeIdName(this XmlElement element)
        => element.GetAttribute(AttributeIdKey);

    public static bool HasAttributeIdValue(this XmlElement element)
        => element.HasAttribute(AttributeIdValue);

    public static string GetAttributeIdValue(this XmlElement element)
        => element.GetAttribute(AttributeIdValue);
}
