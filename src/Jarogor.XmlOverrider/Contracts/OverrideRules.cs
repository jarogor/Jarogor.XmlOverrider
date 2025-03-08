namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
/// Rules
/// </summary>
public sealed class OverrideRules
{
    /// <summary>
    /// Xpath for search
    /// </summary>
    public string XPath { get; set; }

    /// <summary>
    /// Override type
    /// </summary>
    public OverrideType OverrideType { get; set; }

    /// <summary>
    /// Array of attributes names
    /// </summary>
    public string[] Attributes { get; set; }
}