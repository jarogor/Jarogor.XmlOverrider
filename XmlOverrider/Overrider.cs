using System;
using System.IO;
using System.Xml;

using Microsoft.Extensions.Logging;

namespace XmlOverrider;

public abstract class Overrider<T>
{
    protected ILogger? Logger { get; }
    private readonly Markup _markup;

    protected Overrider(ILogger? logger, string markupFilePath, string? schemeFilePath = null)
    {
        _markup = SetMarkup(markupFilePath, schemeFilePath);
        Logger = logger;
    }

    protected Overrider(string markupFilePath, string? schemeFilePath = null)
    {
        _markup = SetMarkup(markupFilePath, schemeFilePath);
    }

    protected abstract XmlDocument TargetXml { get; set; }

    public abstract T Processing();

    protected void Processing(XmlDocument overridingXmlDocument)
    {
        Overriding.Processing(Logger, _markup, overridingXmlDocument, TargetXml);
    }

    public XmlDocument Get()
    {
        return TargetXml;
    }

    private static Markup SetMarkup(string filePath, string? schemeFilePath = null)
    {
        schemeFilePath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Markup.xsd");
        if (!File.Exists(schemeFilePath))
        {
            throw new FileNotFoundException($"XSD scheme file does not exist: [{schemeFilePath}]");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Markup xml file does not exist: [{filePath}]");
        }

        return new Markup(filePath, schemeFilePath);
    }
}