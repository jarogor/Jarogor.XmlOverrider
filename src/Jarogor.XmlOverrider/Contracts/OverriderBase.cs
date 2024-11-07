using System.Xml;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Base overriding logic
/// </summary>
/// <typeparam name="T">Type of overrider</typeparam>
public abstract class OverriderBase<T> {
    private readonly Rules _rules;

    /// <summary>
    ///     Constructor for streams
    /// </summary>
    /// <param name="rules">Overriding rules</param>
    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    protected OverriderBase(Rules rules, ILogger<T> logger) {
        _rules = rules;
        Logger = logger;
    }

    /// <summary>
    ///     Logger
    /// </summary>
    protected ILogger<T> Logger { get; }

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
        => new Overriding<T>(_rules, overridingXmlDocument, TargetXml, Logger).Processing();

    /// <summary>
    ///     Getting the result
    /// </summary>
    /// <returns>Result xml document</returns>
    public XmlDocument Get()
        => TargetXml;
}
