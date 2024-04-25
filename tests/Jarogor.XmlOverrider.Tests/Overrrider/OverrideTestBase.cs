using System.Xml;

namespace Jarogor.XmlOverrider.Tests.Overrrider;

public abstract class OverrideTestBase
{
    protected static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");
    protected static readonly string RulesFilePath = Path.Combine(BasePath, "rules.test.xml");
    protected static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");
    protected static readonly string TargetXmlFilePath = Path.Combine(BasePath, "test.xml");
    protected static readonly string ExpectedFilePath = Path.Combine(BasePath, "expected.xml");

    protected static readonly List<string> FromXmlFiles = new()
    {
        Path.Combine(BasePath, "override-a.xml"),
        Path.Combine(BasePath, "override-b.xml"),
        Path.Combine(BasePath, "override-c.xml"),
    };

    protected static string ExpectedXml => LoadXml(ExpectedFilePath).OuterXml;

    protected static XmlDocument LoadXml(string filePath)
    {
        var xml = new XmlDocument();
        xml.Load(filePath);
        return xml;
    }
}