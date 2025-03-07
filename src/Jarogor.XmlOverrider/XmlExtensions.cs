using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Jarogor.XmlOverrider.Extensions;

namespace Jarogor.XmlOverrider;

internal static class XmlExtensions
{
    public static void OverrideAttributes(
        this XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlElement targetChild
    )
    {
        HashSet<string> hashSet = new(rulesChildNode.RulesAttributeNames());

        foreach (XmlAttribute overrideAttribute in overrideChild.Attributes)
        {
            if (!hashSet.Contains(overrideAttribute.LocalName))
            {
                continue;
            }

            foreach (XmlAttribute targetAttribute in targetChild.Attributes)
            {
                if (targetAttribute.LocalName != overrideAttribute.LocalName)
                {
                    continue;
                }

                if (targetAttribute.Value == overrideAttribute.Value)
                {
                    continue;
                }

                Logger.XmlInformation("Attributes on the element", overrideChild, rulesChildNode);
                targetAttribute.Value = overrideAttribute.Value;
            }
        }
    }

    private static IEnumerable<string> RulesAttributeNames(this XmlNode rules)
    {
        return rules
            .ChildXmlElement(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());
    }

    public static bool IsElementFound(this XmlNode element, string name)
    {
        return element.NodeType == XmlNodeType.Element && element.LocalName == name;
    }

    public static bool IsAttributeIdsEquals(this XmlElement rules, XmlElement @override, XmlElement target)
    {
        if (!rules.HasAttributeIdName() || !@override.HasAttributes)
        {
            return false;
        }

        string key = rules.GetAttributeIdName();
        if (!@override.HasAttribute(key))
        {
            return false;
        }

        string targetValue = target.GetAttribute(key);
        bool isValueEquals = @override.GetAttribute(key) == targetValue;

        if (rules.HasAttributeIdValue())
        {
            return isValueEquals && targetValue == rules.GetAttributeIdValue();
        }

        return isValueEquals;
    }

    public static void ReplaceChildren(this XmlNode target, XmlElement overrideChild, XmlElement targetChild)
    {
        if (target.OwnerDocument is null)
        {
            return;
        }

        XmlNode newNode = target.OwnerDocument.ImportNode(overrideChild, true);
        target.ReplaceChild(newNode, targetChild);
    }

    public static IEnumerable<XmlElement> ChildXmlElement(this XmlNode self)
    {
        return self
            .ChildNodes
            .OfType<XmlElement>();
    }

    public static IEnumerable<XmlElement> ChildXmlElement(this XmlNode self, Func<XmlElement, bool> predicate)
    {
        return self
            .ChildXmlElement()
            .Where(predicate);
    }
}