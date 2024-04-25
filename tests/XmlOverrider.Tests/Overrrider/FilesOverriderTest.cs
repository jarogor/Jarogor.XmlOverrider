using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using XmlOverrider.Overrider;

namespace XmlOverrider.Tests.Overrrider;

public class FilesOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromFilesSuccess()
    {
        var overrider = new FilesOverrider(new NullLogger<FilesOverrider>(), TargetXmlFilePath, RulesFilePath, SchemeFilePath);
        var actual = overrider.AddOverride(FromXmlFiles).Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }
}