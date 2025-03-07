using System.Xml;

using Jarogor.XmlOverrider.Scheme;

namespace Jarogor.XmlOverrider.Tests.OverrideFromFiles;

public abstract class TestBase
{
    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");
    private static readonly string RulesFilePath = Path.Combine(BasePath, "rules.test.xml");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");
    protected static readonly string TargetXmlFilePath = Path.Combine(BasePath, "test.xml");
    private static readonly string ExpectedFilePath = Path.Combine(BasePath, "expected.xml");

    protected static readonly Rules Rules = new(
        new StreamReader(File.OpenRead(RulesFilePath)),
        new StreamReader(File.OpenRead(SchemeFilePath))
    );

    protected static readonly List<string> FromXmlFiles = new()
    {
        Path.Combine(BasePath, "override-a.xml"),
        Path.Combine(BasePath, "override-b.xml"),
        Path.Combine(BasePath, "override-c.xml"),
    };

    protected static string ExpectedXml => LoadXml(ExpectedFilePath).OuterXml;

    protected static XmlDocument LoadXml(string filePath)
    {
        XmlDocument? xml = new();
        xml.Load(filePath);
        return xml;
    }
}