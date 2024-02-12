using System.Xml;

namespace XmlOverrider;

public sealed class Overrider
{
    private readonly string _markupXsdFilePath
        = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheme", "Markup.xsd");

    private readonly Markup _markup;

    private readonly XmlDocument _toXml = new();

    private readonly string _toXmlFilePath;

    private readonly List<string> _fromXmlFilesPaths = new();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="markupFilePath"></param>
    /// <param name="fromXmlFilePath"></param>
    /// <param name="toXmlFilePath"></param>
    /// <param name="markupXsdFilePath"></param>
    public Overrider(
        string fromXmlFilePath,
        string toXmlFilePath,
        string markupFilePath,
        string? markupXsdFilePath = null
    )
    {
        _markup = new Markup(markupFilePath, markupXsdFilePath ?? _markupXsdFilePath);
        _toXml.Load(toXmlFilePath);
        _toXmlFilePath = toXmlFilePath;
        _fromXmlFilesPaths.Add(fromXmlFilePath);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="markupFilePath"></param>
    /// <param name="fromXmlFilesPaths"></param>
    /// <param name="toXmlFilePath"></param>
    /// <param name="markupXsdFilePath"></param>
    public Overrider(
        IEnumerable<string> fromXmlFilesPaths,
        string toXmlFilePath,
        string markupFilePath,
        string? markupXsdFilePath = null
    )
    {
        _markup = new Markup(markupFilePath, markupXsdFilePath ?? _markupXsdFilePath);
        _toXml.Load(toXmlFilePath);
        _toXmlFilePath = toXmlFilePath;
        _fromXmlFilesPaths.AddRange(fromXmlFilesPaths);
    }

    public Overrider Process()
    {
        foreach (var fromXmlPath in _fromXmlFilesPaths)
        {
            var from = new XmlDocument();
            from.Load(fromXmlPath);
            Overriding.Processing(_markup, from, _toXml);
        }

        return this;
    }

    public void Save()
    {
        _toXml.Save(_toXmlFilePath);
    }

    public XmlDocument Get()
    {
        return _toXml;
    }
}