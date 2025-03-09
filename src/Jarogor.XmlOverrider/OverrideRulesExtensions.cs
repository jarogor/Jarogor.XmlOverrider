using System.Text.RegularExpressions;
using System.Xml;

using Jarogor.XmlOverrider.Contracts;

namespace Jarogor.XmlOverrider;

internal static class OverrideRulesExtensions
{
    private static readonly Regex Regex = new(@"@(?<attributeName>\w+)\b[^=]?", RegexOptions.Compiled);

    public static bool IsEquals(this OverrideRules rules, XmlNode a, XmlNode b)
    {
        if (a.Attributes is null || b.Attributes is null)
        {
            return false;
        }

        Match match = Regex.Match(rules.XPath.Expression);
        if (!match.Success)
        {
            return false;
        }

        string key = match.Groups["attributeName"].Value;
        if (a.Attributes[key] is null || b.Attributes[key] is null)
        {
            return false;
        }

        return a.Attributes[key]?.Value == b.Attributes[key]?.Value;
    }
}