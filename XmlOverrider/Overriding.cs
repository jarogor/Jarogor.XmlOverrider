using System.Xml;

using Microsoft.Extensions.Logging;

using XmlOverrider.Extensions;

namespace XmlOverrider;

internal static class Overriding
{
    private static ILogger? s_logger;

    public static void Processing(ILogger? logger, Markup markup, XmlDocument from, XmlDocument to)
    {
        s_logger = logger;

        ProcessingRecursive(
            markup.XmlDocument.DocumentElement ?? throw new InvalidOperationException("markUpXml"),
            from.DocumentElement ?? throw new InvalidOperationException("from"),
            to.DocumentElement ?? throw new InvalidOperationException("to")
        );
    }

    private static void ProcessingRecursive(XmlElement markup, XmlElement from, XmlElement to)
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

            ProcessingChilds(markupChildNode, from, to);
        }
    }

    private static void ProcessingChilds(XmlElement markup, XmlNode from, XmlNode to)
    {
        foreach (var fromChild in from.ChildNodes.OfType<XmlElement>())
        {
            if (fromChild.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            foreach (var toChild in to.ChildNodes.OfType<XmlElement>())
            {
                ProcessingChild(markup, to, fromChild, toChild);
            }
        }
    }

    private static void ProcessingChild(XmlElement markup, XmlNode to, XmlElement fromChild, XmlElement toChild)
    {
        if (!IsElementNamesEquals(markup, fromChild, toChild))
        {
            return;
        }

        if (markup.IsOverridable() && IsAttributeIdsEquals(markup, fromChild, toChild))
        {
            if (markup.IsOverrideInnerXml())
            {
                ReplaceChildren(to, fromChild, toChild);
                s_logger?.LogInformation($"Inner xml of element: {LogHelper.Message(fromChild, markup)}");
                return;
            }

            OverrideAttributes(markup, fromChild, toChild);
        }

        ProcessingRecursive(markup, fromChild, toChild);
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

    private static void OverrideAttributes(XmlElement markup, XmlElement from, XmlElement to)
    {
        foreach (var markupAttrName in MarkupAttributeNames(markup))
        {
            foreach (XmlAttribute overrideAttribute in from.Attributes)
            {
                if (markupAttrName != overrideAttribute.LocalName)
                {
                    continue;
                }

                OverrideAttribute(markup, from, to, overrideAttribute);
            }
        }
    }

    private static void OverrideAttribute(XmlElement markup, XmlElement from, XmlElement to, XmlAttribute fromAttribute)
    {
        foreach (XmlAttribute toAttribute in to.Attributes)
        {
            if (toAttribute.LocalName != fromAttribute.LocalName)
            {
                continue;
            }

            if (toAttribute.Value == fromAttribute.Value)
            {
                continue;
            }

            s_logger?.LogInformation($"Attributes on the element: {LogHelper.Message(from, markup)}");
            toAttribute.Value = fromAttribute.Value;
        }
    }

    private static IEnumerable<string> MarkupAttributeNames(XmlNode markup)
    {
        return markup.ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());
    }

    private static bool IsElementNamesEquals(XmlElement markup, XmlNode from, XmlNode to)
    {
        if (!markup.IsElementType())
        {
            return false;
        }

        var name = markup.GetElementName();

        return IsElementFound(from, name) && IsElementFound(to, name);
    }

    private static bool IsElementFound(XmlNode element, string name)
    {
        return element.NodeType == XmlNodeType.Element && element.LocalName == name;
    }

    private static bool IsAttributeIdsEquals(XmlElement markup, XmlElement from, XmlElement to)
    {
        // Не определён в разметке идентифицирующий атрибут или отсутствуют атрибуты у перекрывающего конфига.
        if (!markup.HasAttributeIdName() || !from.HasAttributes)
        {
            return false;
        }

        var key = markup.GetAttributeIdName();

        // В перекрывающем конфиге отсутствует указанный в разметке идентифицирующий атрибут.
        if (!from.HasAttribute(key))
        {
            return false;
        }

        var targetValue = to.GetAttribute(key);
        var isValueEquals = from.GetAttribute(key) == targetValue;

        // Если указано сравнение по конкретному значению идентифицирующего атрибута.
        if (markup.HasAttributeIdValue())
        {
            return isValueEquals && targetValue == markup.GetAttributeIdValue();
        }

        return isValueEquals;
    }
}