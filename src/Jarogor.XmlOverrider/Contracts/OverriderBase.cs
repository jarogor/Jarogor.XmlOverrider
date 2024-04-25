using System.IO;
using System.Xml;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider.Contracts;

public abstract class OverriderBase<T>
{
    protected ILogger<T> Logger { get; }
    private readonly Rules _rules;

    protected OverriderBase(
        ILogger<T> logger,
        string rulesFilePath,
        string? schemeFilePath = null
    )
    {
        _rules = new Rules(rulesFilePath, schemeFilePath);
        Logger = logger;
    }

    protected OverriderBase(
        ILogger<T> logger,
        TextReader rulesStream,
        TextReader? schemeStream = null
    )
    {
        _rules = new Rules(rulesStream, schemeStream);
        Logger = logger;
    }

    protected abstract XmlDocument TargetXml { get; set; }

    public abstract T Processing();

    protected void Processing(XmlDocument overridingXmlDocument)
    {
        new Overriding<T>(Logger, _rules, overridingXmlDocument, TargetXml).Processing();
    }

    public XmlDocument Get()
    {
        return TargetXml;
    }
}