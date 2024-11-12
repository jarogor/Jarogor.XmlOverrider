using System.Xml;
using Jarogor.XmlOverrider.Overrider;
using Jarogor.XmlOverrider.Scheme;
using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderInnerXmlByKeyAndValue {
    private const string RulesXml =
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

    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");

    [Test]
    public void Success() {
        var target = new XmlDocument();
        target.LoadXml(SourceXml);

        var rules = new Rules(new StringReader(RulesXml), SchemeFilePath);
        var overrider = new StringOverrider(rules, target.OuterXml);

        var overridingXmlDocument = new XmlDocument();
        overridingXmlDocument.LoadXml(OverridingXml);

        var expected = new XmlDocument();
        expected.LoadXml(ExpectedXml);

        overrider.AddOverride(overridingXmlDocument);
        var actual = overrider.Processing().Get();

        Assert.That(actual.OuterXml, Is.EqualTo(expected.OuterXml));
    }
}
