using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

using Jarogor.XmlOverrider.Extensions;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding<T>
{
    private readonly ILogger<T> _logger;
    private readonly XmlElement _rules;
    private readonly XmlElement _from;
    private readonly XmlElement _target;

    public Overriding(ILogger<T> logger, Rules rules, XmlDocument from, XmlDocument target)
    {
        _logger = logger;
        _rules = rules.XmlDocument.DocumentElement ?? throw new InvalidOperationException("rules xml");
        _from = from.DocumentElement ?? throw new InvalidOperationException("from xml");
        _target = target.DocumentElement ?? throw new InvalidOperationException("target xml");
    }

    public void Processing()
    {
        ProcessingRecursive(_rules, _from, _target);
    }

    private void ProcessingRecursive(XmlNode rules, XmlNode from, XmlNode target)
    {
        if (!rules.HasChildNodes || !from.HasChildNodes)
        {
            return;
        }

        foreach (var rulesChildNode in rules.ChildNodes.OfType<XmlElement>())
        {
            var name = rulesChildNode.GetElementName();
            if (from[name] == null)
            {
                continue;
            }

            ProcessingChilds(rulesChildNode, from, target);
        }
    }

    private void ProcessingChilds(XmlElement rules, XmlNode from, XmlNode target)
    {
        foreach (var fromChild in from.ChildNodes.OfType<XmlElement>())
        {
            if (fromChild.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            foreach (var targetChild in target.ChildNodes.OfType<XmlElement>())
            {
                ProcessingChild(rules, target, fromChild, targetChild);
            }
        }
    }

    private void ProcessingChild(XmlElement rules, XmlNode target, XmlElement fromChild, XmlElement targetChild)
    {
        if (!IsElementNamesEquals(rules, fromChild, targetChild))
        {
            return;
        }

        if (rules.IsOverridable() && IsAttributeIdsEquals(rules, fromChild, targetChild))
        {
            if (rules.IsOverrideInnerXml())
            {
                ReplaceChildren(target, fromChild, targetChild);
                _logger.LogInformation($"Inner xml of element: {LogHelper.Message(fromChild, rules)}");
                return;
            }

            OverrideAttributes(rules, fromChild, targetChild.Attributes);
        }

        ProcessingRecursive(rules, fromChild, targetChild);
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

    private void OverrideAttributes(XmlElement rules, XmlElement from, XmlAttributeCollection targetAttributes)
    {
        foreach (var rulesAttrName in RulesAttributeNames(rules))
        {
            foreach (XmlAttribute fromAttribute in from.Attributes)
            {
                if (rulesAttrName != fromAttribute.LocalName)
                {
                    continue;
                }

                OverrideAttribute(rules, from, targetAttributes, fromAttribute);
            }
        }
    }

    private void OverrideAttribute(XmlElement rules, XmlElement from, XmlAttributeCollection targetAttributes, XmlAttribute fromAttribute)
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

            _logger.LogInformation($"Attributes on the element: {LogHelper.Message(from, rules)}");
            targetAttribute.Value = fromAttribute.Value;
        }
    }

    private static IEnumerable<string> RulesAttributeNames(XmlNode rules)
    {
        return rules.ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());
    }

    private static bool IsElementNamesEquals(XmlElement rules, XmlNode from, XmlNode target)
    {
        if (!rules.IsElementType())
        {
            return false;
        }

        var name = rules.GetElementName();

        return IsElementFound(from, name) && IsElementFound(target, name);
    }

    private static bool IsElementFound(XmlNode element, string name)
    {
        return element.NodeType == XmlNodeType.Element && element.LocalName == name;
    }

    private static bool IsAttributeIdsEquals(XmlElement rules, XmlElement from, XmlElement target)
    {
        if (!rules.HasAttributeIdName() || !from.HasAttributes)
        {
            return false;
        }

        var key = rules.GetAttributeIdName();

        if (!from.HasAttribute(key))
        {
            return false;
        }

        var targetValue = target.GetAttribute(key);
        var isValueEquals = from.GetAttribute(key) == targetValue;

        if (rules.HasAttributeIdValue())
        {
            return isValueEquals && targetValue == rules.GetAttributeIdValue();
        }

        return isValueEquals;
    }
}