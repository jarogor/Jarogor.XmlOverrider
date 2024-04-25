using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.Overrrider;

public class XmlDocumentOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromXmlDocumentSuccess()
    {
        var overrider = new XmlDocumentOverrider(new NullLogger<XmlDocumentOverrider>(), LoadXml(TargetXmlFilePath), RulesFilePath, SchemeFilePath);
        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }
}