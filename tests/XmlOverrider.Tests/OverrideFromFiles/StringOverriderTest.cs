using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.OverrideFromFiles;

[TestFixture]
public class StringOverriderTest : TestBase {
    [Test]
    public void OverridingFromStringSuccess() {
        var overrider = new StringOverrider(Rules, LoadXml(TargetXmlFilePath).OuterXml);

        foreach (var file in FromXmlFiles) {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}
