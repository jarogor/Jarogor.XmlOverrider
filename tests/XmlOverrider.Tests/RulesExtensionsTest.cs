using System.Xml;

using NUnit.Framework;

using XmlOverrider.Extensions;

namespace XmlOverrider.Tests;

public class RulesExtensionsTest
{
    private const string Xml
        = """
          <node name="foo"
               attributeIdName="bar"
               attributeIdValue="qwerty"
               override="innerXml"
          />
          """;

    [Test]
    public void OverridingRulesExtensionsTest()
    {
        var xml = new XmlDocument();
        xml.LoadXml(Xml);

        var element = xml.DocumentElement;

        Assert.Multiple(() =>
        {
            Assert.That(element!.IsAttributeElement(), Is.False);
            Assert.That(element!.IsElementType(), Is.True);
            Assert.That(element!.GetAttributeIdName(), Is.EqualTo("bar"));
            Assert.That(element!.HasAttributeIdName(), Is.True);
            Assert.That(element!.GetElementName(), Is.EqualTo("foo"));
            Assert.That(element!.HasAttributeIdValue(), Is.True);
            Assert.That(element!.GetAttributeIdValue(), Is.EqualTo("qwerty"));
            Assert.That(element!.IsOverridable(), Is.True);
            Assert.That(element!.IsOverrideInnerXml(), Is.True);
        });
    }
}