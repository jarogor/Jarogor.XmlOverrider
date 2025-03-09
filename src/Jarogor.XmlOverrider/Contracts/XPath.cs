namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     XPath expression
/// </summary>
public struct XPath
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="expression"></param>
    public XPath(string expression)
    {
        Expression = expression;
    }

    /// <summary>
    ///     XPath expression
    /// </summary>
    public string Expression { get; }
}