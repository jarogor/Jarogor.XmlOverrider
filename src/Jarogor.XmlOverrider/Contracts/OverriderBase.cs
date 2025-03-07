using System.Xml;

using Jarogor.XmlOverrider.Scheme;

using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Base overriding logic
/// </summary>
/// <typeparam name="T">Type of overrider</typeparam>
public abstract class OverriderBase<T>
{
    private readonly Rules _rules;

    /// <summary>
    ///     Constructor for streams
    /// </summary>
    /// <param name="rules">Overriding rules</param>
    protected OverriderBase(Rules rules)
    {
        _rules = rules;
    }

    /// <summary>
    ///     XML to override
    /// </summary>
    protected abstract XmlDocument TargetXml { get; set; }

    /// <summary>
    ///     Processing of override
    /// </summary>
    /// <returns>Type of overrider</returns>
    public abstract T Processing();

    /// <summary>
    ///     Processing of override by xml
    /// </summary>
    /// <param name="overridingXmlDocument">overriding xml</param>
    protected void Processing(XmlDocument overridingXmlDocument)
    {
        new Overriding(_rules, overridingXmlDocument, TargetXml).Processing();
    }

    /// <summary>
    ///     Getting the result
    /// </summary>
    /// <returns>Result xml document</returns>
    public XmlDocument Get()
    {
        return TargetXml;
    }
}