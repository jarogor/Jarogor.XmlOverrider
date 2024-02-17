using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Microsoft.Extensions.Logging;

using XmlOverrider.Extensions;

namespace XmlOverrider;

internal static class Overriding
{
    private static ILogger? s_logger;

    public static void Processing(ILogger? logger, Markup markup, XmlDocument from, XmlDocument target)
    {
        s_logger = logger;

        ProcessingRecursive(
            markup.XmlDocument.DocumentElement ?? throw new InvalidOperationException("markup xml"),
            from.DocumentElement ?? throw new InvalidOperationException("from xml"),
            target.DocumentElement ?? throw new InvalidOperationException("target xml")
        );
    }

    private static void ProcessingRecursive(XmlNode markup, XmlNode from, XmlNode target)
    {
        if (!markup.HasChildNodes || !from.HasChildNodes)
        {
            return;
        }

        foreach (var markupChildNode in markup.ChildNodes.OfType<XmlElement>())
        {
            var name = markupChildNode.GetElementName();
            if (from[name] == null)
            {
                continue;
            }

            ProcessingChilds(markupChildNode, from, target);
        }
    }

    private static void ProcessingChilds(XmlElement markup, XmlNode from, XmlNode target)
    {
        foreach (var fromChild in from.ChildNodes.OfType<XmlElement>())
        {
            if (fromChild.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            foreach (var targetChild in target.ChildNodes.OfType<XmlElement>())
            {
                ProcessingChild(markup, target, fromChild, targetChild);
            }
        }
    }

    private static void ProcessingChild(XmlElement markup, XmlNode target, XmlElement fromChild, XmlElement targetChild)
    {
        if (!IsElementNamesEquals(markup, fromChild, targetChild))
        {
            return;
        }

        if (markup.IsOverridable() && IsAttributeIdsEquals(markup, fromChild, targetChild))
        {
            if (markup.IsOverrideInnerXml())
            {
                ReplaceChildren(target, fromChild, targetChild);
                s_logger?.LogInformation($"Inner xml of element: {LogHelper.Message(fromChild, markup)}");
                return;
            }

            OverrideAttributes(markup, fromChild, targetChild.Attributes);
        }

        ProcessingRecursive(markup, fromChild, targetChild);
    }

    private static void ReplaceChildren(XmlNode parent, XmlNode newChild, XmlNode oldChild)
    {
        if (parent.OwnerDocument == null)
        {
            return;
        }

        var newNode = parent.OwnerDocument.ImportNode(newChild, true);
        parent.ReplaceChild(newNode, oldChild);
    }

    private static void OverrideAttributes(XmlElement markup, XmlElement from, XmlAttributeCollection targetAttributes)
    {
        foreach (var markupAttrName in MarkupAttributeNames(markup))
        {
            foreach (XmlAttribute fromAttribute in from.Attributes)
            {
                if (markupAttrName != fromAttribute.LocalName)
                {
                    continue;
                }

                OverrideAttribute(markup, from, targetAttributes, fromAttribute);
            }
        }
    }

    private static void OverrideAttribute(XmlElement markup, XmlElement from, XmlAttributeCollection targetAttributes, XmlAttribute fromAttribute)
    {
        foreach (XmlAttribute targetAttribute in targetAttributes)
        {
            if (targetAttribute.LocalName != fromAttribute.LocalName)
            {
                continue;
            }

            if (targetAttribute.Value == fromAttribute.Value)
            {
                continue;
            }

            s_logger?.LogInformation($"Attributes on the element: {LogHelper.Message(from, markup)}");
            targetAttribute.Value = fromAttribute.Value;
        }
    }

    private static IEnumerable<string> MarkupAttributeNames(XmlNode markup)
    {
        return markup.ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());
    }

    private static bool IsElementNamesEquals(XmlElement markup, XmlNode from, XmlNode target)
    {
        if (!markup.IsElementType())
        {
            return false;
        }

        var name = markup.GetElementName();

        return IsElementFound(from, name) && IsElementFound(target, name);
    }

    private static bool IsElementFound(XmlNode element, string name)
    {
        return element.NodeType == XmlNodeType.Element && element.LocalName == name;
    }

    private static bool IsAttributeIdsEquals(XmlElement markup, XmlElement from, XmlElement target)
    {
        if (!markup.HasAttributeIdName() || !from.HasAttributes)
        {
            return false;
        }

        var key = markup.GetAttributeIdName();

        if (!from.HasAttribute(key))
        {
            return false;
        }

        var targetValue = target.GetAttribute(key);
        var isValueEquals = from.GetAttribute(key) == targetValue;

        if (markup.HasAttributeIdValue())
        {
            return isValueEquals && targetValue == markup.GetAttributeIdValue();
        }

        return isValueEquals;
    }
}