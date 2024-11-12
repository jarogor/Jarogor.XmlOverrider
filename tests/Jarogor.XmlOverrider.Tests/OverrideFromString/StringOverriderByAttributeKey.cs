using System.Xml;
using Jarogor.XmlOverrider.Overrider;
using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderByAttributeKey : TestBase {
    protected override string RulesXml =>
        """
        <?xml version="1.0" encoding="utf-8"?>
        <overrideRules>
            <node name="section-c">
                <node name="item" attributeIdName="key" override="attributes">
                    <attribute name="value"/>
                </node>
            </node>
        </overrideRules>
        """;

    private const string SourceXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-c>
                <item key="a" value="1"/>
            </section-c>
        </root>
        """;

    private const string OverridingXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-c>
                <item key="a" value="new"/>
            </section-c>
        </root>
        """;

    private const string ExpectedXml = OverridingXml;

    [Test]
    public void Success() {
        var target = new XmlDocument();
        target.LoadXml(SourceXml);

        var overridingXmlDocument = new XmlDocument();
        overridingXmlDocument.LoadXml(OverridingXml);

        var expected = new XmlDocument();
        expected.LoadXml(ExpectedXml);

        var overrider = new StringOverrider(Rules(), target.OuterXml);
        overrider.AddOverride(overridingXmlDocument);

        var actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(expected.OuterXml));
    }
}
