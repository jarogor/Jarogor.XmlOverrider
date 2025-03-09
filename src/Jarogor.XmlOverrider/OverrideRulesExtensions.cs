using System.Text.RegularExpressions;
using System.Xml;

using Jarogor.XmlOverrider.Contracts;

namespace Jarogor.XmlOverrider;

internal static class OverrideRulesExtensions
{
    private static readonly Regex Regex = new(@"@(?<attributeName>\w+)\b[^=]?", RegexOptions.Compiled);

    public static void OverrideAttributes(this OverrideRules setting, XmlNode targetNode, XmlNode overrideNode)
    {
        if (targetNode.Attributes is null || overrideNode.Attributes is null)
        {
            return;
        }

        Match match = Regex.Match(setting.XPath.Expression);
        if (!match.Success)
        {
            return;
        }

        string key = match.Groups["attributeName"].Value;
        if (targetNode.Attributes[key] is null || overrideNode.Attributes[key] is null)
        {
            return;
        }

        if (targetNode.Attributes[key]?.Value != overrideNode.Attributes[key]?.Value)
        {
            return;
        }

        foreach (string name in setting.Attributes)
        {
            if (targetNode.Attributes?[name] is not null && overrideNode.Attributes?[name] is not null)
            {
                targetNode.Attributes[name]!.Value = overrideNode.Attributes[name]!.Value;
            }
        }
    }
}