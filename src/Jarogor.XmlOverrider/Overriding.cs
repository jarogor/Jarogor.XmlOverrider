using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Jarogor.XmlOverrider.Extensions;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding<T> {
    private readonly ILogger<T> _logger;

    private readonly XmlElement _override;
    private readonly XmlElement _rules;
    private readonly XmlElement _target;

    public Overriding(
        Rules rules,
        XmlDocument @override,
        XmlDocument target,
        ILogger<T> logger
    ) {
        _logger = logger;
        _rules = rules.XmlDocument.DocumentElement ?? throw new InvalidOperationException("rules xml");
        _override = @override.DocumentElement ?? throw new InvalidOperationException("override xml");
        _target = target.DocumentElement ?? throw new InvalidOperationException("target xml");
    }

    public void Processing()
        => ProcessingRecursive(_rules, _override, _target);

    private void ProcessingRecursive(
        XmlNode rules,
        XmlNode @override,
        XmlNode target
    ) {
        if (!rules.HasChildNodes || !@override.HasChildNodes) {
            return;
        }

        var rulesChildNodes = rules
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => @override[it.GetElementName()] is not null)
            .ToList();

        var overrideChildren = @override
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.NodeType == XmlNodeType.Element)
            .ToList();

        var targetChildren = target
            .ChildNodes
            .OfType<XmlElement>()
            .ToList();

        foreach (var rulesChildNode in rulesChildNodes) {
            foreach (var overrideChild in overrideChildren) {
                ProcessingChild(rulesChildNode, overrideChild, target, targetChildren);
            }
        }
    }

    private void ProcessingChild(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlNode target,
        IEnumerable<XmlElement> targetChildren
    ) {
        var targets = targetChildren
            .Where(it => IsElementNamesEquals(rulesChildNode, overrideChild, it))
            .ToList();

        foreach (var targetChild in targets) {
            if (rulesChildNode.IsOverridable() && IsAttributeIdsEquals(rulesChildNode, overrideChild, targetChild)) {
                if (rulesChildNode.IsOverrideInnerXml()) {
                    ReplaceChildren(overrideChild, target, targetChild);
                    _logger.LogInformation("Inner xml of element: {0}", LogHelper.Message(overrideChild, rulesChildNode));
                } else {
                    OverrideAttributes(rulesChildNode, overrideChild, targetChild);
                }
            } else {
                ProcessingRecursive(rulesChildNode, overrideChild, targetChild);
            }
        }
    }

    private static void ReplaceChildren(
        XmlElement overrideChild,
        XmlNode target,
        XmlElement targetChild
    ) {
        if (target.OwnerDocument is null) {
            return;
        }

        var newNode = target.OwnerDocument.ImportNode(overrideChild, true);
        target.ReplaceChild(newNode, targetChild);
    }

    private void OverrideAttributes(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlElement targetChild
    ) {
        foreach (var rulesAttrName in RulesAttributeNames(rulesChildNode)) {
            foreach (XmlAttribute overrideAttribute in overrideChild.Attributes) {
                if (rulesAttrName != overrideAttribute.LocalName) {
                    continue;
                }

                OverrideAttribute(rulesChildNode, overrideChild, overrideAttribute, targetChild);
            }
        }
    }

    private void OverrideAttribute(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlAttribute overrideAttribute,
        XmlElement targetChild
    ) {
        foreach (XmlAttribute targetAttribute in targetChild.Attributes) {
            if (targetAttribute.LocalName != overrideAttribute.LocalName) {
                continue;
            }

            if (targetAttribute.Value == overrideAttribute.Value) {
                continue;
            }

            _logger.LogInformation("Attributes on the element: {0}", LogHelper.Message(overrideChild, rulesChildNode));
            targetAttribute.Value = overrideAttribute.Value;
        }
    }

    private static IEnumerable<string> RulesAttributeNames(XmlNode rules)
        => rules
            .ChildNodes
            .OfType<XmlElement>()
            .Where(it => it.IsAttributeElement())
            .Select(it => it.GetElementName());

    private static bool IsElementNamesEquals(
        XmlElement rules,
        XmlNode @override,
        XmlNode target
    ) {
        if (!rules.IsElementType()) {
            return false;
        }

        var name = rules.GetElementName();

        return IsElementFound(@override, name)
               && IsElementFound(target, name);
    }

    private static bool IsElementFound(XmlNode element, string name)
        => element.NodeType == XmlNodeType.Element
           && element.LocalName == name;

    private static bool IsAttributeIdsEquals(
        XmlElement rules,
        XmlElement @override,
        XmlElement target
    ) {
        if (!rules.HasAttributeIdName() || !@override.HasAttributes) {
            return false;
        }

        var key = rules.GetAttributeIdName();
        if (!@override.HasAttribute(key)) {
            return false;
        }

        var targetValue = target.GetAttribute(key);
        var isValueEquals = @override.GetAttribute(key) == targetValue;

        if (rules.HasAttributeIdValue()) {
            return isValueEquals && targetValue == rules.GetAttributeIdValue();
        }

        return isValueEquals;
    }
}
