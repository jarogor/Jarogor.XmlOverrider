using System.Xml;
using NUnit.Framework;
using XmlOverrider.Overrider;
using XmlOverrider.Scheme;

namespace XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderByAttributeKey {
    private const string RulesXml =
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
    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");

    [Test]
    public void Success() {
        var target = new XmlDocument();
        target.LoadXml(SourceXml);

        var rules = new Rules(new StringReader(RulesXml), new StreamReader(File.OpenRead(SchemeFilePath)));
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
