using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Jarogor.XmlOverrider.Scheme;

/// <summary>
///     Override rules
/// </summary>
public sealed class Rules {
    /// <summary>
    ///     Override rules
    /// </summary>
    public readonly XmlDocument XmlDocument = new();

    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="xsdFilePath">Path to the override rules schema file</param>
    /// <exception cref="FileNotFoundException"></exception>
    public static Rules Create(string rulesFilePath, string? xsdFilePath = null) {
        xsdFilePath ??= XsdFilePath();
        var rulesStream = new StreamReader(File.OpenRead(FilePathValidate(rulesFilePath)));
        var xsdStream = new StreamReader(File.OpenRead(xsdFilePath));
        return new Rules(rulesStream, xsdStream);
    }

    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="xsdStream">Override rules schema stream</param>
    public static Rules Create(TextReader rulesStream, TextReader? xsdStream = null)
        => new(rulesStream, xsdStream ?? new StreamReader(File.OpenRead(XsdFilePath())));

    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="xsdStream">Override rules schema stream</param>
    public static Rules Create(string rulesFilePath, TextReader? xsdStream = null) {
        var rulesStream = new StreamReader(File.OpenRead(FilePathValidate(rulesFilePath)));
        xsdStream ??= new StreamReader(File.OpenRead(XsdFilePath()));
        return new Rules(rulesStream, xsdStream);
    }

    /// <param name="rulesStream">Override rules stream</param>
    /// <param name="xsdFilePath">Path to the override rules schema file</param>
    public static Rules Create(TextReader rulesStream, string? xsdFilePath = null)
        => new(rulesStream, new StreamReader(File.OpenRead(xsdFilePath ?? XsdFilePath())));

    private Rules(TextReader rulesStream, TextReader xsdStream) {
        using var schemaDocument = XmlReader.Create(xsdStream);
        var schemas = new XmlSchemaSet();
        schemas.Add(string.Empty, schemaDocument);

        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemas);
        settings.ValidationType = ValidationType.Schema;

        using var reader = XmlReader.Create(rulesStream, settings);
        XmlDocument.Load(reader);
        XmlDocument.Validate((_, e) => throw e.Exception);
    }

    private static string FilePathValidate(string rulesFilePath) {
        if (!File.Exists(rulesFilePath)) {
            throw new FileNotFoundException($"Rules xml file does not exist: [{rulesFilePath}]");
        }

        return rulesFilePath;
    }

    private static string XsdFilePath() {
        var xsdFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Rules.xsd");
        if (!File.Exists(xsdFilePath)) {
            throw new FileNotFoundException($"XSD scheme file does not exist: [{xsdFilePath}]");
        }

        return xsdFilePath;
    }
}
