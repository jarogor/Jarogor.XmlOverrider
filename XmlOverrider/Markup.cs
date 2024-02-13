using System.Xml;
using System.Xml.Schema;

namespace XmlOverrider;

internal sealed class Markup
{
    public readonly XmlDocument XmlDocument = new();

    public Markup(
        string markupFilePath,
        string xsdFilePath)
    {
        var settings = ValidationSettings(xsdFilePath);
        var markupReader = XmlReader.Create(markupFilePath, settings);

        XmlDocument.Load(markupReader);
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