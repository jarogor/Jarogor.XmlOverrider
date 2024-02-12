using System.Xml;
using XmlOverrider.Extensions;

namespace XmlOverrider;

public static class Overriding
{
    public static void Processing(Markup markup, XmlDocument from, XmlDocument to)
    {
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

            ProcessingChildNodes(markupChildNode, from, to);
        }
    }

    private static void ProcessingChildNodes(XmlElement markup, XmlNode from, XmlNode to)
    {
        foreach (var fromChild in from.ChildNodes.OfType<XmlElement>())
        {
            if (fromChild.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            foreach (var toChild in to.ChildNodes.OfType<XmlElement>())
            {
                if (!IsElementNamesEquals(markup, fromChild, toChild))
                {
                    continue;
                }

                if (markup.IsOverridable() && IsAttributeIdsEquals(markup, fromChild, toChild))
                {
                    if (markup.IsOverrideInnerXml())
                    {
                        ReplaceChildren(to, fromChild, toChild);
                        continue;
                    }

                    OverrideAttributes(markup, fromChild, toChild);
                }

                ProcessingRecursive(markup, fromChild, toChild);
            }
        }
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
        var markupAttributeNames = markup.ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName())
            .ToList();

        foreach (var markupAttrName in markupAttributeNames)
        {
            foreach (XmlAttribute overrideAttribute in from.Attributes)
            {
                if (markupAttrName != overrideAttribute.LocalName)
                {
                    continue;
                }

                foreach (XmlAttribute targetAttribute in to.Attributes)
                {
                    if (targetAttribute.LocalName != overrideAttribute.LocalName)
                    {
                        continue;
                    }

                    if (targetAttribute.Value == overrideAttribute.Value)
                    {
                        continue;
                    }

                    targetAttribute.Value = overrideAttribute.Value;
                }
            }
        }
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