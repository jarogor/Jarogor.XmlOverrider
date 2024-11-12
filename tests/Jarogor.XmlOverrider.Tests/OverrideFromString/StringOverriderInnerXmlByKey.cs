﻿using System.Xml;
using Jarogor.XmlOverrider.Overrider;
using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public class StringOverriderInnerXmlByKey : TestBase {
    protected override string RulesXml =>
        """
        <?xml version="1.0" encoding="utf-8"?>
        <overrideRules>
            <node name="section-b">
                <node name="item" attributeIdName="key" override="innerXml"/>
            </node>
        </overrideRules>
        """;

    private const string SourceXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-b>
                <item key="a">
                    <a a="a"/>
                </item>
                <item key="b">
                    <a a="a"/>
                </item>
            </section-b>
        </root>
        """;

    private const string OverridingXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <root>
            <section-b>
                <item key="a">
                    <new name="new"/>
                </item>
                <item key="b">
                    <new name="new"/>
                </item>
            </section-b>
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
