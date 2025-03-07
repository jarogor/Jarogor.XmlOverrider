using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Jarogor.XmlOverrider.Extensions;
using Jarogor.XmlOverrider.Scheme;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding
{
    private readonly XmlElement _override;
    private readonly XmlElement _rules;
    private readonly XmlElement _target;

    public Overriding(Rules rules, XmlDocument @override, XmlDocument target)
    {
        _rules = rules.XmlDocument.DocumentElement ?? throw new InvalidOperationException("rules xml");
        _override = @override.DocumentElement ?? throw new InvalidOperationException("override xml");
        _target = target.DocumentElement ?? throw new InvalidOperationException("target xml");
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
            .ChildXmlElement(it => @override[it.GetElementName()] is not null)
            .ToList();

        List<XmlElement> overrideChildren = @override
            .ChildXmlElement(it => it.NodeType == XmlNodeType.Element)
            .ToList();

        List<XmlElement> targetChildren = target
            .ChildXmlElement()
            .ToList();

        foreach (XmlElement? rulesChildNode in rulesChildNodes)
        {
            bool isElementType = rulesChildNode.IsElementType();
            string name = rulesChildNode.GetElementName();
            if (!isElementType)
            {
                continue;
            }

            foreach (XmlElement? overrideChild in overrideChildren)
            {
                IEnumerable<XmlElement> targets = targetChildren
                    .Where(it => it.IsElementFound(name))
                    .Where(_ => overrideChild.IsElementFound(name));

                bool isOverridable = rulesChildNode.IsOverridable();
                bool isOverrideInnerXml = rulesChildNode.IsOverrideInnerXml();
                foreach (XmlElement? targetChild in targets)
                {
                    if (isOverridable && rulesChildNode.IsAttributeIdsEquals(overrideChild, targetChild))
                    {
                        if (isOverrideInnerXml)
                        {
                            target.ReplaceChildren(overrideChild, targetChild);
                            Logger.XmlInformation("Inner xml of element", overrideChild, rulesChildNode);
                        }
                        else
                        {
                            OverrideAttributes(rulesChildNode, overrideChild, targetChild);
                        }

                        continue;
                    }

                    ProcessingRecursive(rulesChildNode, overrideChild, targetChild);
                }
            }
        }
    }

    private void OverrideAttributes(
        XmlElement rulesChildNode,
        XmlElement overrideChild,
        XmlElement targetChild
    )
    {
        var hashSet = new HashSet<string>(rulesChildNode.RulesAttributeNames());

        foreach (XmlAttribute overrideAttribute in overrideChild.Attributes)
        {
            if (hashSet.Contains(overrideAttribute.LocalName))
            {
                OverrideAttribute(rulesChildNode, overrideChild, overrideAttribute, targetChild);
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

            Logger.XmlInformation("Attributes on the element", overrideChild, rulesChildNode);
            targetAttribute.Value = overrideAttribute.Value;
        }
    }
}