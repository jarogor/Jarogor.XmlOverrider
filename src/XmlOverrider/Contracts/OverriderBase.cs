using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using XmlOverrider.Scheme;

namespace XmlOverrider.Contracts;

/// <summary>
/// Base overriding logic
/// </summary>
/// <typeparam name="T">Type of overrider</typeparam>
public abstract class OverriderBase<T>
{
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger<T> Logger { get; }
    private readonly Rules _rules;

    /// <summary>
    /// Constructor for files paths
    /// </summary>
    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
    protected OverriderBase(
        ILogger<T> logger,
        string rulesFilePath,
        string? schemeFilePath = null
    )
    {
        _rules = new Rules(rulesFilePath, schemeFilePath);
        Logger = logger;
    }

    /// <summary>
    /// Constructor for streams
    /// </summary>
    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="schemeStream">Override rules schema stream</param>
    protected OverriderBase(
        ILogger<T> logger,
        TextReader rulesStream,
        TextReader? schemeStream = null
    )
    {
        _rules = new Rules(rulesStream, schemeStream);
        Logger = logger;
    }

    /// <summary>
    /// XML to override
    /// </summary>
    protected abstract XmlDocument TargetXml { get; set; }

    /// <summary>
    /// Processing of override
    /// </summary>
    /// <returns>Type of overrider</returns>
    public abstract T Processing();

    /// <summary>
    /// Processing of override by xml
    /// </summary>
    /// <param name="overridingXmlDocument">overriding xml</param>
    protected void Processing(XmlDocument overridingXmlDocument)
    {
        new Overriding<T>(Logger, _rules, overridingXmlDocument, TargetXml).Processing();
    }

    /// <summary>
    /// Getting the result
    /// </summary>
    /// <returns>Result xml document</returns>
    public XmlDocument Get()
    {
        return TargetXml;
    }
}