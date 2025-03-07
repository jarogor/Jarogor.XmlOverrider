using System.Xml;

using Jarogor.XmlOverrider.Extensions;

namespace Jarogor.XmlOverrider;

internal static class LogHelper
{
    public static string Message(XmlElement element, XmlElement rules)
    {
        string? key = rules.GetAttributeIdName();

        return $"{XPath(element)}[@{key}='{element.GetAttribute(key)}']";
    }

    private static string XPath(XmlNode node)
    {
        if (node.NodeType != XmlNodeType.Attribute)
        {
            return GetPath(node);
        }

        XmlElement? ownerElement = ((XmlAttribute)node).OwnerElement;
        return ownerElement is not null
            ? $"{XPath(ownerElement)}"
            : GetPath(node);
    }

    private static string GetPath(XmlNode node)
    {
        return node.ParentNode is not null
            ? $"{XPath(node.ParentNode)}/{node.Name}"
            : "/";
    }
}