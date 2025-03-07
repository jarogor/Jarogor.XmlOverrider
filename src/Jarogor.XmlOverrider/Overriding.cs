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

    private static void ProcessingRecursive(
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
            .Where(it => it.IsElementType())
            .ToList();

        List<XmlElement> overrideChildren = @override
            .ChildXmlElement(it => it.NodeType == XmlNodeType.Element)
            .ToList();

        List<XmlElement> targetChildren = target
            .ChildXmlElement()
            .ToList();

        foreach (XmlElement? rulesChildNode in rulesChildNodes)
        {
            string name = rulesChildNode.GetElementName();
            bool isOverridable = rulesChildNode.IsOverridable();
            bool isOverrideInnerXml = rulesChildNode.IsOverrideInnerXml();

            foreach (XmlElement? overrideChild in overrideChildren)
            {
                IEnumerable<XmlElement> targets = targetChildren
                    .Where(it => it.IsElementFound(name))
                    .Where(_ => overrideChild.IsElementFound(name));

                foreach (XmlElement? targetChild in targets)
                {
                    if (isOverridable && rulesChildNode.IsAttributeIdsEquals(overrideChild, targetChild))
                    {
                        Override(isOverrideInnerXml, target, overrideChild, targetChild, rulesChildNode);
                        continue;
                    }

                    ProcessingRecursive(rulesChildNode, overrideChild, targetChild);
                }
            }
        }
    }

    private static void Override(bool isOverrideInnerXml, XmlNode target, XmlElement overrideChild, XmlElement targetChild, XmlElement rulesChildNode)
    {
        if (isOverrideInnerXml)
        {
            target.ReplaceChildren(overrideChild, targetChild);
            Logger.XmlInformation("Inner xml of element", overrideChild, rulesChildNode);
        }
        else
        {
            rulesChildNode.OverrideAttributes(overrideChild, targetChild);
        }
    }
}