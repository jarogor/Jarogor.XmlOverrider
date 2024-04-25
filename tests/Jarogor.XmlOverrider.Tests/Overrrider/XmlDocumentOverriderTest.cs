using Jarogor.XmlOverrider.Overrider;
using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.Overrrider;

public class XmlDocumentOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromXmlDocumentSuccess()
    {
        var overrider = new XmlDocumentOverrider(
            LoadXml(TargetXmlFilePath),
            RulesFilePath,
            SchemeFilePath
        );

        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}