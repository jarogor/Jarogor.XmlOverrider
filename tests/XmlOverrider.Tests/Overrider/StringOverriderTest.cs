using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.Overrider;

public class StringOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromStringSuccess()
    {
        var overrider = new StringOverrider(s_rules, LoadXml(TargetXmlFilePath).OuterXml);

        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}