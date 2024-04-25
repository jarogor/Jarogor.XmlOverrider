using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Jarogor.XmlOverrider.Overrider;

namespace Jarogor.XmlOverrider.Tests.Overrrider;

public class StringOverriderTest : OverrideTestBase
{
    [Test]
    public void OverridingFromStringSuccess()
    {
        var overrider = new StringOverrider(new NullLogger<StringOverrider>(), LoadXml(TargetXmlFilePath).OuterXml, RulesFilePath, SchemeFilePath);
        foreach (var file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        var actual = overrider.Processing().Get();
        var expected = LoadXml(ExpectedFilePath).OuterXml;
        Assert.That(actual.OuterXml, Is.EqualTo(expected));
    }
}