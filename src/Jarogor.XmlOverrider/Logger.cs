using System.Xml;

using Jarogor.XmlOverrider.Extensions;

using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider;

internal static class Logger
{
    public static ILogger? Log { get; set; }

    public static void XmlInformation(string? message, XmlElement element, XmlElement rules)
    {
        Log?.LogInformation("{Name}: {0}", message, Message(element, rules));
    }

    private static string Message(XmlElement element, XmlElement rules)
    {
        string key = rules.GetAttributeIdName();
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