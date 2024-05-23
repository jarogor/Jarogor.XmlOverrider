using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.Overrider;

public class XmlDocumentOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromXmlDocumentSuccess()
    {
        var overrider = new XmlDocumentOverrider(s_rules, LoadXml(TargetXmlFilePath));

        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}