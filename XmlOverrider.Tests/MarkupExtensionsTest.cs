using System.Xml;

using XmlOverrider.Extensions;

using Xunit;

namespace XmlOverrider.Tests;

public class MarkupExtensionsTest
{
    private const string Xml
        = """
          <node name="foo"
               attributeIdName="bar"
               attributeIdValue="qwerty"
               override="innerXml"
          />
          """;

    [Fact]
    public void OverridingConfigsMarkupExtensionsTest()
    {
        var element = Load();

        Assert.False(element!.IsAttributeElement());
        Assert.True(element.IsElementType());
        Assert.Equal("bar", element.GetAttributeIdName());

        Assert.True(element.HasAttributeIdName());
        Assert.Equal("foo", element.GetElementName());

        Assert.True(element.HasAttributeIdValue());
        Assert.Equal("qwerty", element.GetAttributeIdValue());

        Assert.True(element.IsOverridable());
        Assert.True(element.IsOverrideInnerXml());
    }

    private static XmlElement Load()
    {
        var xml = new XmlDocument();
        xml.LoadXml(Xml);

        return xml.DocumentElement;
    }
}