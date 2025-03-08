using System.Xml;

using BenchmarkDotNet.Attributes;

using Jarogor.XmlOverrider.Overrider;

namespace Jarogor.XmlOverrider.Benchmarks;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[ExceptionDiagnoser]
public class StringOverriderBenchmark
{
    private static readonly OverrideRules[] Rules =
    [
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
    ];

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
            <section-b>
                <item key="a">
                    <a a="a"/>
                </item>
                <item key="b">
                    <a a="a"/>
                </item>
            </section-b>
            <section-c>
                <item key="a" value="1"/>
            </section-c>
            <section-d>
                <item key="a" value="1"/>
            </section-d>
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
            <section-b>
                <item key="a">
                    <new name="new"/>
                </item>
                <item key="b">
                    <new name="new"/>
                </item>
            </section-b>
            <section-c>
                <item key="a" value="new"/>
            </section-c>
        </root>
        """;

    [Benchmark]
    public void Benchmark()
    {
        XmlDocument target = new();
        target.LoadXml(SourceXml);

        XmlDocument overridingXmlDocument = new();
        overridingXmlDocument.LoadXml(OverridingXml);

        StringOverrider overrider = new(Rules, target.OuterXml);
        overrider.AddOverride(overridingXmlDocument);

        overrider.Processing().Get();
    }
}