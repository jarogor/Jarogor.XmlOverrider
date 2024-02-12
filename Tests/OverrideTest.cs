using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using XmlOverrider;
using Xunit;

namespace Tests;

public class OverrideTest
{
    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");

    private static readonly string MarkupFilePath = Path.Combine(BasePath, "markup.test.xml");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Markup.xsd");
    private static readonly string ToXmlFilePath = Path.Combine(BasePath, "test.xml");
    private static readonly string ExpectedFilePath = Path.Combine(BasePath, "expected.xml");

    private static readonly List<string> FromXmlFiles = new()
    {
        Path.Combine(BasePath, "override-a.xml"),
        Path.Combine(BasePath, "override-b.xml"),
        Path.Combine(BasePath, "override-c.xml"),
    };

    [Fact]
    public void OverridingConfigsTest2()
    {
        var expectedXml = new XmlDocument();
        expectedXml.Load(ExpectedFilePath);
        var overrider = new Overrider(FromXmlFiles, ToXmlFilePath, MarkupFilePath, SchemeFilePath);

        Assert.Equal(expectedXml.OuterXml, overrider.Process().Get().OuterXml);
    }
}