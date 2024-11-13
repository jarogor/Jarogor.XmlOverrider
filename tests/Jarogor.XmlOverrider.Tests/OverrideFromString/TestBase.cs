using Jarogor.XmlOverrider.Scheme;
using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromString;

[TestFixture]
public abstract class TestBase {
    private static readonly string BasePath = Path.Combine(Environment.CurrentDirectory, "data");
    private static readonly string SchemeFilePath = Path.Combine(BasePath, "Rules.xsd");

    protected abstract string RulesXml { get; }

    protected Rules Rules()
        => new(
            new StringReader(RulesXml),
            new StreamReader(File.OpenRead(SchemeFilePath))
        );
}
