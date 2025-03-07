using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Jarogor.XmlOverrider.Scheme;

/// <summary>
///     Override rules
/// </summary>
public sealed class Rules
{
    /// <summary>
    ///     Override rules
    /// </summary>
    public readonly XmlDocument XmlDocument = new();

    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="xsdStream">Override rules schema stream</param>
    public Rules(TextReader rulesStream, TextReader? xsdStream = null)
    {
        xsdStream ??= new StreamReader(File.OpenRead(XsdFilePath()));
        using XmlReader? schemaDocument = XmlReader.Create(xsdStream);
        XmlSchemaSet? schemas = new();
        schemas.Add(string.Empty, schemaDocument);

        XmlReaderSettings? settings = new();
        settings.Schemas.Add(schemas);
        settings.ValidationType = ValidationType.Schema;

        using XmlReader? reader = XmlReader.Create(rulesStream, settings);
        XmlDocument.Load(reader);
        XmlDocument.Validate((_, e) => throw e.Exception);
    }

    private static string XsdFilePath()
    {
        string? xsdFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Rules.xsd");

        if (!File.Exists(xsdFilePath))
        {
            throw new FileNotFoundException($"XSD scheme file does not exist: [{xsdFilePath}]");
        }

        return xsdFilePath;
    }
}