using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Jarogor.XmlOverrider.Extensions;
using Jarogor.XmlOverrider.Scheme;

using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding<T>
{
    private readonly ILogger<T> _logger;
    private readonly XmlElement _override;
    private readonly XmlElement _rules;
    private readonly XmlElement _target;

    public Overriding(
        Rules rules,
        XmlDocument @override,
        XmlDocument target,
        ILogger<T> logger
    )
    {
        _rules = rules.XmlDocument.DocumentElement ?? throw new InvalidOperationException("rules xml");
        _override = @override.DocumentElement ?? throw new InvalidOperationException("override xml");
        _target = target.DocumentElement ?? throw new InvalidOperationException("target xml");
        _logger = logger;
    }

    public void Processing()
    {
        ProcessingRecursive(_rules, _override, _target);
    }

    private void ProcessingRecursive(
        XmlNode rules,
        XmlNode @override,
        XmlNode target
    )
    {
        if (!rules.HasChildNodes || !@override.HasChildNodes)
        {
            return;
        }

        List<XmlElement> rulesChildNodes = rules
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => @override[it.GetElementName()] is not null)
            .ToList();

        List<XmlElement> overrideChildren = @override
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.NodeType == XmlNodeType.Element)
            .ToList();

        List<XmlElement> targetChildren = target
            .ChildNodes
            .OfType<XmlElement>()
            .ToList();

        foreach (XmlElement? rulesChildNode in rulesChildNodes)
        {
            bool isElementType = rulesChildNode.IsElementType();
            string name = rulesChildNode.GetElementName();

            foreach (XmlElement? overrideChild in overrideChildren)
            {
                IEnumerable<XmlElement> targets = targetChildren.Where(it => isElementType && IsElementNamesEquals(name, overrideChild, it));

                foreach (XmlElement? targetChild in targets)
                {
                    if (rulesChildNode.IsOverridable() && IsAttributeIdsEquals(rulesChildNode, overrideChild, targetChild))
                    {
                        Override(target, rulesChildNode, overrideChild, targetChild);
                        continue;
                    }

                    ProcessingRecursive(rulesChildNode, overrideChild, targetChild);
                }
            }
        }
    }

    private void Override(
        XmlNode target,
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlElement targetChild
    )
    {
        if (rulesChildNode.IsOverrideInnerXml())
        {
            ReplaceChildren(overrideChild, target, targetChild);
            _logger.LogInformation("Inner xml of element: {0}", LogHelper.Message(overrideChild, rulesChildNode));
            return;
        }

        OverrideAttributes(rulesChildNode, overrideChild, targetChild);
    }

    private static void ReplaceChildren(
        XmlElement overrideChild,
        XmlNode target,
        XmlElement targetChild
    )
    {
        if (target.OwnerDocument is null)
        {
            return;
        }

        XmlNode newNode = target.OwnerDocument.ImportNode(overrideChild, true);
        target.ReplaceChild(newNode, targetChild);
    }

    private void OverrideAttributes(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlElement targetChild
    )
    {
        foreach (string? rulesAttrName in RulesAttributeNames(rulesChildNode))
        {
            foreach (XmlAttribute overrideAttribute in overrideChild.Attributes)
            {
                if (rulesAttrName == overrideAttribute.LocalName)
                {
                    OverrideAttribute(rulesChildNode, overrideChild, overrideAttribute, targetChild);
                }
            }
        }
    }

    private void OverrideAttribute(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlAttribute overrideAttribute,
        XmlElement targetChild
    )
    {
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

            _logger.LogInformation("Attributes on the element: {0}", LogHelper.Message(overrideChild, rulesChildNode));
            targetAttribute.Value = overrideAttribute.Value;
        }
    }

    private static IEnumerable<string> RulesAttributeNames(XmlNode rules)
    {
        return rules
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());
    }

    private static bool IsElementNamesEquals(string name, XmlNode @override, XmlNode target)
    {
        return IsElementFound(@override, name) && IsElementFound(target, name);
    }

    private static bool IsElementFound(XmlNode element, string name)
    {
        return element.NodeType == XmlNodeType.Element && element.LocalName == name;
    }

    private static bool IsAttributeIdsEquals(
        XmlElement rules,
        XmlElement @override,
        XmlElement target
    )
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
}