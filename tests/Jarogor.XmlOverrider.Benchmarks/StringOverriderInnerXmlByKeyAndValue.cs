using System.Runtime.CompilerServices;
using System.Xml;
using BenchmarkDotNet.Attributes;
using Jarogor.XmlOverrider.Overrider;
using Jarogor.XmlOverrider.Scheme;

namespace Jarogor.XmlOverrider.Benchmarks;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[ExceptionDiagnoser]
public class StringOverriderInnerXmlByKeyAndValue {
    private static string BasePath([CallerFilePath] string path = "") => path;
    private const string RulesXsd = @"..\..\src\Jarogor.XmlOverrider\Scheme\Rules.xsd";

    [Benchmark]
    public void Benchmark() {
        var basePath = Path.GetDirectoryName(BasePath());
        var fileStream = File.OpenRead(Path.Combine(basePath ?? ".", RulesXsd));
        var rules = new Rules(new StringReader(RulesXml), new StreamReader(fileStream));

        var target = new XmlDocument();
        target.LoadXml(SourceXml);

        var overridingXmlDocument = new XmlDocument();
        overridingXmlDocument.LoadXml(OverridingXml);

        var overrider = new StringOverrider(rules, target.OuterXml);
        overrider.AddOverride(overridingXmlDocument);

        overrider.Processing().Get();
    }

    private const string RulesXml =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <overrideRules>
            <node name="section-a">
                <node name="item" attributeIdName="key" attributeIdValue="b" override="innerXml"/>
            </node>
            <node name="section-b">
                <node name="item" attributeIdName="key" override="innerXml"/>
            </node>
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
}
