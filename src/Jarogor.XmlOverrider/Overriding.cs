using System;
using System.Xml;

using Jarogor.XmlOverrider.Contracts;

namespace Jarogor.XmlOverrider;

internal sealed class Overriding
{
    private readonly XmlElement _overrides;
    private readonly OverrideRules[] _rules;
    private readonly XmlElement _target;

    public Overriding(OverrideRules[] rules, XmlDocument overrides, XmlDocument target)
    {
        _rules = rules ?? throw new InvalidOperationException("rules");
        _target = target.DocumentElement ?? throw new InvalidOperationException("target");
        _overrides = overrides.DocumentElement ?? throw new InvalidOperationException("overrides");
    }

    public void Processing()
    {
        foreach (OverrideRules rules in _rules)
        {
            XmlNodeList? targetFounds = _target.SelectNodes(rules.XPath.Expression);
            if (targetFounds is null)
            {
                continue;
            }

            XmlNodeList? overridesFounds = _overrides.SelectNodes(rules.XPath.Expression);
            if (overridesFounds is null)
            {
                continue;
            }

            foreach (XmlNode targetNode in targetFounds)
            {
                foreach (XmlNode overrideNode in overridesFounds)
                {
                    Handle(rules, targetNode, overrideNode);
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