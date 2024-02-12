using System.Xml;

namespace XmlOverrider.Extensions;

public static class MarkupXmlElementExtensions
{
    private const string Name = "name";

    private const string TypeAttribute = "attribute";

    private const string AttributeIdKey = "attributeIdName";
    private const string AttributeIdValue = "attributeIdValue";

    private const string AttributeOverride = "override";
    private const string OverrideInnerXml = "innerXml";

    public static string GetElementName(this XmlElement element)
    {
        return element.GetAttribute(Name);
    }

    public static bool IsOverridable(this XmlElement element)
    {
        return element.HasAttribute(AttributeOverride);
    }

    public static bool IsOverrideInnerXml(this XmlElement element)
    {
        return element.Attributes[AttributeOverride]?.Value == OverrideInnerXml;
    }

    public static bool IsAttributeElement(this XmlElement element)
    {
        return element.LocalName == TypeAttribute;
    }

    public static bool IsElementType(this XmlElement element)
    {
        return element.LocalName != TypeAttribute;
    }

    public static bool HasAttributeIdName(this XmlElement element)
    {
        return element.HasAttribute(AttributeIdKey);
    }

    public static string GetAttributeIdName(this XmlElement element)
    {
        return element.GetAttribute(AttributeIdKey);
    }

    public static bool HasAttributeIdValue(this XmlElement element)
    {
        return element.HasAttribute(AttributeIdValue);
    }

    public static string GetAttributeIdValue(this XmlElement element)
    {
        return element.GetAttribute(AttributeIdValue);
    }
}