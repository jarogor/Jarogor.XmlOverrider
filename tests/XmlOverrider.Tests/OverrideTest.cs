using System.Xml;

using NUnit.Framework;

namespace XmlOverrider.Tests;

public class OverrideTest
{
    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");

    private static readonly string RulesFilePath = Path.Combine(BasePath, "rules.test.xml");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");
    private static readonly string TargetXmlFilePath = Path.Combine(BasePath, "test.xml");
    private static readonly string ExpectedFilePath = Path.Combine(BasePath, "expected.xml");

    private static readonly List<string> FromXmlFiles = new()
    {
        Path.Combine(BasePath, "override-a.xml"),
        Path.Combine(BasePath, "override-b.xml"),
        Path.Combine(BasePath, "override-c.xml"),
    };

    [Test]
    public void OverridingFromFilesSuccess()
    {
        var overrider = new FilesOverrider(TargetXmlFilePath, RulesFilePath, SchemeFilePath);
        var actual = overrider.AddOverride(FromXmlFiles).Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }

    [Test]
    public void OverridingFromXmlDocumentSuccess()
    {
        var overrider = new XmlDocumentOverrider(LoadXml(TargetXmlFilePath), RulesFilePath, SchemeFilePath);
        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }

    [Test]
    public void OverridingFromStringSuccess()
    {
        var overrider = new StringOverrider(LoadXml(TargetXmlFilePath).OuterXml, RulesFilePath, SchemeFilePath);
        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }

    private static XmlDocument LoadXml(string filePath)
    {
        var expectedXml = new XmlDocument();
        expectedXml.Load(filePath);
        return expectedXml;
    }
}