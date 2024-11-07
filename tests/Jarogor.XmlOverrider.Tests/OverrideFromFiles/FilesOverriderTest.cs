using NUnit.Framework;
using Jarogor.XmlOverrider.Overrider;

namespace Jarogor.XmlOverrider.Tests.OverrideFromFiles;

[TestFixture]
public class FilesOverriderTest : TestBase {
    [Test]
    public void OverridingFromFilesSuccess() {
        var overrider = new FilesOverrider(Rules, TargetXmlFilePath);

        var actual = overrider
            .AddOverride(FromXmlFiles)
            .Processing()
            .Get();

        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}
