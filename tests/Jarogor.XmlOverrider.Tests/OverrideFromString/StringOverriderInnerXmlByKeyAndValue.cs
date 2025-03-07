using System.Xml;

using Jarogor.XmlOverrider.Overrider;

using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderInnerXmlByKeyAndValue : TestBase
{
    protected override string RulesXml =>
        """
        <?xml version="1.0" encoding="utf-8"?>
        <overrideRules>
            <node name="section-a">
                <node name="item" attributeIdName="key" attributeIdValue="b" override="innerXml"/>
            </node>
        </overrideRules>
        """;

    private const string SourceXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a">
                    <a a="a"/>
                </item>
                <item key="b">
                    <a a="a"/>
                </item>
            </section-a>
        </root>
        """;

    private const string OverridingXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a">
                    <new name="new"/>
                </item>
                <item key="b">
                    <new name="new"/>
                </item>
            </section-a>
        </root>
        """;

    private const string ExpectedXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-a>
                <item key="a">
                    <a a="a"/>
                </item>
                <item key="b">
                    <new name="new"/>
                </item>
            </section-a>
        </root>
        """;

    [Test]
    public void Success()
    {
        XmlDocument? target = new();
        target.LoadXml(SourceXml);

        XmlDocument? overridingXmlDocument = new();
        overridingXmlDocument.LoadXml(OverridingXml);

        XmlDocument? expected = new();
        expected.LoadXml(ExpectedXml);

        StringOverrider? overrider = new(Rules(), target.OuterXml);
        overrider.AddOverride(overridingXmlDocument);

        XmlDocument? actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(expected.OuterXml));
    }
}