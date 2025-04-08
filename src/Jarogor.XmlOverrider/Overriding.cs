using System;
using System.Xml;

using Jarogor.XmlOverrider.Contracts;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding(OverrideRules[] rules, XmlDocument overrides, XmlDocument target)
{
    private readonly XmlElement _overrides = overrides.DocumentElement ?? throw new ArgumentNullException(nameof(overrides));
    private readonly OverrideRules[] _rules = rules ?? throw new ArgumentNullException(nameof(rules));
    private readonly XmlElement _target = target.DocumentElement ?? throw new ArgumentNullException(nameof(target));

    public void Processing()
    {
        foreach (OverrideRules overrideRules in _rules)
        {
            XmlNodeList? targetFounds = _target.SelectNodes(overrideRules.XPath.Expression);
            if (targetFounds is null)
            {
                continue;
            }

            XmlNodeList? overridesFounds = _overrides.SelectNodes(overrideRules.XPath.Expression);
            if (overridesFounds is null)
            {
                continue;
            }

            foreach (XmlNode targetNode in targetFounds)
            {
                foreach (XmlNode overrideNode in overridesFounds)
                {
                    Handle(overrideRules, targetNode, overrideNode);
                }
            }
        }
    }

    private static void Handle(OverrideRules rules, XmlNode targetNode, XmlNode overrideNode)
    {
        if (!rules.IsEquals(targetNode, overrideNode))
        {
            return;
        }

        switch (rules.OverrideType)
        {
            case OverrideType.Attributes:
                foreach (string name in rules.Attributes)
                {
                    if (targetNode.Attributes?[name] is not null && overrideNode.Attributes?[name] is not null)
                    {
                        targetNode.Attributes[name]!.Value = overrideNode.Attributes[name]!.Value;
                    }
                }

                break;

            case OverrideType.InnerXml:
                targetNode.InnerXml = overrideNode.InnerXml;
                break;

            case OverrideType.InnerText:
                targetNode.InnerText = overrideNode.InnerText;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(rules), "Override type is not supported");
        }
    }
}