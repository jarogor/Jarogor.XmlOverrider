using XmlOverrider.Overrider;
using NUnit.Framework;

namespace XmlOverrider.Tests.Overrrider;

public class FilesOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromFilesSuccess()
    {
        var overrider = new FilesOverrider(
            TargetXmlFilePath,
            RulesFilePath,
            SchemeFilePath
        );

        var actual = overrider
            .AddOverride(FromXmlFiles)
            .Processing()
            .Get();

        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}