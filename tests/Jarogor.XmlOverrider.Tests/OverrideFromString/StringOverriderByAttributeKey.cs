using System.Xml;

using Jarogor.XmlOverrider.Overrider;

using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderByAttributeKey : TestBase
{
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