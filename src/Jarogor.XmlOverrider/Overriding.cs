using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Jarogor.XmlOverrider.Extensions;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding<T> {
    private readonly XmlElement _override;
    private readonly ILogger<T> _logger;
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

        foreach (var rulesChildNode in rules.ChildNodes.OfType<XmlElement>()) {
            var name = rulesChildNode.GetElementName();
            if (@override[name] == null) {
                continue;
            }

            ProcessingChildren(rulesChildNode, @override, target);
        }
    }

    private void ProcessingChildren(
        XmlElement rules,
        XmlNode @override,
        XmlNode target
    ) {
        foreach (var overrideChild in @override.ChildNodes.OfType<XmlElement>()) {
            if (overrideChild.NodeType != XmlNodeType.Element) {
                continue;
            }

            foreach (var targetChild in target.ChildNodes.OfType<XmlElement>()) {
                ProcessingChild(rules, target, overrideChild, targetChild);
            }
        }
    }

    private void ProcessingChild(
        XmlElement rules,
        XmlNode target,
        XmlElement overrideChild,
        XmlElement targetChild
    ) {
        if (!IsElementNamesEquals(rules, overrideChild, targetChild)) {
            return;
        }

        if (rules.IsOverridable() && IsAttributeIdsEquals(rules, overrideChild, targetChild)) {
            if (rules.IsOverrideInnerXml()) {
                ReplaceChildren(target, overrideChild, targetChild);
                _logger.LogInformation("Inner xml of element: {0}", LogHelper.Message(overrideChild, rules));
                return;
            }

            OverrideAttributes(rules, overrideChild, targetChild.Attributes);
        }

        ProcessingRecursive(rules, overrideChild, targetChild);
    }

    private static void ReplaceChildren(
        XmlNode parent,
        XmlNode newChild,
        XmlNode oldChild
    ) {
        if (parent.OwnerDocument == null) {
            return;
        }

        var newNode = parent.OwnerDocument.ImportNode(newChild, true);

        parent.ReplaceChild(newNode, oldChild);
    }

    private void OverrideAttributes(
        XmlElement rules,
        XmlElement @override,
        XmlAttributeCollection targetAttributes
    ) {
        foreach (var rulesAttrName in RulesAttributeNames(rules)) {
            foreach (XmlAttribute overrideAttribute in @override.Attributes) {
                if (rulesAttrName != overrideAttribute.LocalName) {
                    continue;
                }

                OverrideAttribute(rules, @override, targetAttributes, overrideAttribute);
            }
        }
    }

    private void OverrideAttribute(
        XmlElement rules,
        XmlElement @override,
        XmlAttributeCollection targetAttributes,
        XmlAttribute overrideAttribute
    ) {
        foreach (XmlAttribute targetAttribute in targetAttributes) {
            if (targetAttribute.LocalName != overrideAttribute.LocalName) {
                continue;
            }

            if (targetAttribute.Value == overrideAttribute.Value) {
                continue;
            }

            _logger.LogInformation("Attributes on the element: {0}", LogHelper.Message(@override, rules));
            targetAttribute.Value = overrideAttribute.Value;
        }
    }

    private static IEnumerable<string> RulesAttributeNames(XmlNode rules)
        => rules.ChildNodes
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
