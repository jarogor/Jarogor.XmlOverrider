using System;
using System.IO;
using System.Xml;

using Microsoft.Extensions.Logging;

namespace XmlOverrider;

public abstract class Overrider<T>
{
    protected ILogger? Logger { get; }
    private readonly Rules _rules;

    protected Overrider(ILogger? logger, string rulesFilePath, string? schemeFilePath = null)
    {
        _rules = SetRules(rulesFilePath, schemeFilePath);
        Logger = logger;
    }

    protected Overrider(string rulesFilePath, string? schemeFilePath = null)
    {
        _rules = SetRules(rulesFilePath, schemeFilePath);
    }

    protected abstract XmlDocument TargetXml { get; set; }

    public abstract T Processing();

    protected void Processing(XmlDocument overridingXmlDocument)
    {
        Overriding.Processing(Logger, _rules, overridingXmlDocument, TargetXml);
    }

    public XmlDocument Get()
    {
        return TargetXml;
    }

    private static Rules SetRules(string filePath, string? schemeFilePath = null)
    {
        schemeFilePath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Rules.xsd");
        if (!File.Exists(schemeFilePath))
        {
            throw new FileNotFoundException($"XSD scheme file does not exist: [{schemeFilePath}]");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Rules xml file does not exist: [{filePath}]");
        }

        return new Rules(filePath, schemeFilePath);
    }
}