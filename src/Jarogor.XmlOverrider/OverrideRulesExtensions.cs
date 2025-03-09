using System.Text.RegularExpressions;
using System.Xml;

using Jarogor.XmlOverrider.Contracts;

namespace Jarogor.XmlOverrider;

internal static class OverrideRulesExtensions
{
    private static readonly Regex Regex = new(@"@(?<attributeName>\w+)\b[^=]?", RegexOptions.Compiled);

    public static bool IsEquals(this OverrideRules setting, XmlNode targetNode, XmlNode overrideNode)
    {
        if (targetNode.Attributes is null || overrideNode.Attributes is null)
        {
            return false;
        }

        Match match = Regex.Match(setting.XPath.Expression);
        if (!match.Success)
        {
            return false;
        }

        string key = match.Groups["attributeName"].Value;
        if (targetNode.Attributes[key] is null || overrideNode.Attributes[key] is null)
        {
            return false;
        }

        return targetNode.Attributes[key]?.Value == overrideNode.Attributes[key]?.Value;
    }
}