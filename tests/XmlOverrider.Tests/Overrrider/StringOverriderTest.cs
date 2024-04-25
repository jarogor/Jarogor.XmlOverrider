using XmlOverrider.Overrider;
using NUnit.Framework;

namespace XmlOverrider.Tests.Overrrider;

public class StringOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromStringSuccess()
    {
        var overrider = new StringOverrider(
            LoadXml(TargetXmlFilePath).OuterXml,
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