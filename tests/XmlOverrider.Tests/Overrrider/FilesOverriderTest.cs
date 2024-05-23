using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.Overrrider;

public class FilesOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromFilesSuccess()
    {
        var overrider = new FilesOverrider(s_rules, TargetXmlFilePath);

        var actual = overrider
            .AddOverride(FromXmlFiles)
            .Processing()
            .Get();

        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}