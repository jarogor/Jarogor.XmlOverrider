using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XmlOverrider.Scheme;

internal sealed class Rules
{
    public readonly XmlDocument XmlDocument = new();

    public Rules(string rulesFilePath, string? xsdFilePath = null)
    {
        if (!File.Exists(rulesFilePath))
        {
            throw new FileNotFoundException($"Rules xml file does not exist: [{rulesFilePath}]");
        }

        xsdFilePath ??= XsdFilePath();
        var rulesStream = new StreamReader(File.OpenRead(rulesFilePath));
        var xsdStream = new StreamReader(File.OpenRead(xsdFilePath));
        LoadAndValidate(rulesStream, xsdStream);
    }

    public Rules(TextReader rulesStream, TextReader? xsdStream = null)
    {
        var xsdFilePath = XsdFilePath();
        xsdStream ??= new StreamReader(File.OpenRead(xsdFilePath));
        LoadAndValidate(rulesStream, xsdStream);
    }

    private void LoadAndValidate(TextReader rulesStream, TextReader xsdStream)
    {
        var schemaDocument = XmlReader.Create(xsdStream);
        var schemas = new XmlSchemaSet();
        schemas.Add(string.Empty, schemaDocument);

        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemas);
        settings.ValidationType = ValidationType.Schema;

        var reader = XmlReader.Create(rulesStream, settings);
        XmlDocument.Load(reader);
        XmlDocument.Validate((_, e) => throw e.Exception);
    }

    private static string XsdFilePath()
    {
        var xsdFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Rules.xsd");
        if (!File.Exists(xsdFilePath))
        {
            throw new FileNotFoundException($"XSD scheme file does not exist: [{xsdFilePath}]");
        }

        return xsdFilePath;
    }
}