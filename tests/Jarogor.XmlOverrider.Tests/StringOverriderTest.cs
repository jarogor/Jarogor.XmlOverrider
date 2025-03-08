using System.Xml;

using Jarogor.XmlOverrider.Contracts;
using Jarogor.XmlOverrider.Overrider;

using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests;

[TestFixture]
public class StringOverriderTest
{
    private static readonly OverrideRules[] Rules =
    {
        new()
        {
            XPath = "//section-a/item[@key='b']",
            OverrideType = OverrideType.InnerXml,
        },
        new()
        {
            XPath = "//section-b/item[@key]",
            OverrideType = OverrideType.InnerXml,
        },
        new()
        {
            XPath = "//section-c/item[@key]",
            OverrideType = OverrideType.Attributes,
            Attributes = new[] { "value" },
        },
    };

    private const string SourceXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a"><a a="a"/></item>
                <item key="b"><a a="a"/></item>
            </section-a>
            <section-b>
                <item key="a"><a a="a"/></item>
                <item key="b"><a a="a"/></item>
            </section-b>
            <section-c><item key="a" value="1"/></section-c>
            <section-d><item key="a" value="1"/></section-d>
        </root>
        """;

    private const string OverridingXmlA =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a"><new name="new"/></item>
                <item key="b"><new name="new"/></item>
            </section-a>
        </root>
        """;

    private const string OverridingXmlB =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-b>
                <item key="a"><new name="new1"/></item>
                <item key="b"><new name="new2"/></item>
            </section-b>
        </root>
        """;

    private const string OverridingXmlC =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-c><item key="a" value="new"/></section-c>
        </root>
        """;

    private const string ExpectedXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a"><a a="a"/></item>
                <item key="b"><new name="new"/></item>
            </section-a>
            <section-b>
                <item key="a"><new name="new2"/></item>
                <item key="b"><new name="new2"/></item>
            </section-b>
            <section-c><item key="a" value="new"/></section-c>
            <section-d><item key="a" value="1"/></section-d>
        </root>
        """;

    [Test]
    public void Success()
    {
        XmlDocument target = new();
        target.LoadXml(SourceXml);

        XmlDocument expected = new();
        expected.LoadXml(ExpectedXml);

        StringOverrider? overrider = new(Rules, target.OuterXml);
        XmlDocument overridingA = new();
        overridingA.LoadXml(OverridingXmlA);
        overrider.AddOverride(overridingA);

        XmlDocument overridingB = new();
        overridingB.LoadXml(OverridingXmlB);
        overrider.AddOverride(overridingB);

        XmlDocument overridingC = new();
        overridingC.LoadXml(OverridingXmlC);
        overrider.AddOverride(overridingC);

        XmlDocument actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(expected.OuterXml));
    }
}