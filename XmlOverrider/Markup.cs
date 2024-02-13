using System.Xml;
using System.Xml.Schema;

namespace XmlOverrider;

public class Markup
{
    public readonly XmlDocument XmlDocument = new();

    public Markup(string markupFilePath, string xsdFilePath)
    {
        var settings = ValidationSettings(xsdFilePath);
        var markupReader = XmlReader.Create(markupFilePath, settings);

        XmlDocument.Load(markupReader);
        XmlDocument.Validate(ValidationEventHandler);
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

    private static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        switch (e.Severity)
        {
            case XmlSeverityType.Error:
                Console.WriteLine("Error: {0}", e.Message);
                break;
            case XmlSeverityType.Warning:
                Console.WriteLine("Warning {0}", e.Message);
                break;
            default:
                throw new ArgumentOutOfRangeException(e.Severity.ToString());
        }
    }
}