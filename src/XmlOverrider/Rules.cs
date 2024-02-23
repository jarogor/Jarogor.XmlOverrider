using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XmlOverrider;

internal sealed class Rules
{
    public readonly XmlDocument XmlDocument = new();

    public Rules(
        string rulesFilePath,
        string xsdFilePath)
    {
        var settings = ValidationSettings(xsdFilePath);
        var reader = XmlReader.Create(rulesFilePath, settings);

        XmlDocument.Load(reader);
        XmlDocument.Validate((_, e) => throw e.Exception);
    }

    private static XmlReaderSettings ValidationSettings(string xsdFilePath)
    {
        var stream = File.OpenRead(xsdFilePath);
        var schemaDocument = XmlReader.Create(stream);

        var schemas = new XmlSchemaSet();
        schemas.Add(string.Empty, schemaDocument);

        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemas);
        settings.ValidationType = ValidationType.Schema;

        return settings;
    }
}