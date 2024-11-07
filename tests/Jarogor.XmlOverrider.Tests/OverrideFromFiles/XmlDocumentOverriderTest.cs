using NUnit.Framework;
using Jarogor.XmlOverrider.Overrider;

namespace Jarogor.XmlOverrider.Tests.OverrideFromFiles;

[TestFixture]
public class XmlDocumentOverriderTest : TestBase {
    [Test]
    public void OverridingFromXmlDocumentSuccess() {
        var overrider = new XmlDocumentOverrider(Rules, LoadXml(TargetXmlFilePath));

        foreach (var file in FromXmlFiles) {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}
