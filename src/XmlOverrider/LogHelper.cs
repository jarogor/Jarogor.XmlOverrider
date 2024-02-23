using System.Xml;

using XmlOverrider.Extensions;

namespace XmlOverrider;

internal static class LogHelper
{
    public static string Message(XmlElement element, XmlElement markup)
    {
        var key = markup.GetAttributeIdName();

        return $"{XPath(element)}[@{key}='{element.GetAttribute(key)}']";
    }

    private static string XPath(XmlNode node)
    {
        if (node.NodeType != XmlNodeType.Attribute)
        {
            return GetPath(node);
        }

        var ownerElement = ((XmlAttribute)node).OwnerElement;
        return ownerElement is not null
            ? $"{XPath(ownerElement)}"
            : GetPath(node);
    }

    private static string GetPath(XmlNode node)
        => node.ParentNode is not null
            ? $"{XPath(node.ParentNode)}/{node.Name}"
            : "/";
}