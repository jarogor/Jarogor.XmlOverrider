using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Jarogor.XmlOverrider.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jarogor.XmlOverrider.Overrider;

public sealed class FilesOverrider : OverriderBase<FilesOverrider>, IFilesOverrider<FilesOverrider>
{
    private readonly string _targetXmlFilePath;
    private readonly List<string> _overrideXmlFilesPaths = new();

    /// <param name="logger">Microsoft.Extensions.Logging.ILogger implementation</param>
    /// <param name="targetXmlFilePath">Path to the xml file that needs to be overridden</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
    public FilesOverrider(
        ILogger<FilesOverrider> logger,
        string targetXmlFilePath,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        logger,
        rulesFilePath,
        schemeFilePath
    )
    {
        _targetXmlFilePath = SetTarget(targetXmlFilePath);
        TargetXml.Load(_targetXmlFilePath);
    }

    /// <param name="targetXmlFilePath">Path to the xml file that needs to be overridden</param>
    /// <param name="rulesFilePath">Path to override rules file</param>
    /// <param name="schemeFilePath">Path to the override rules schema file</param>
    public FilesOverrider(
        string targetXmlFilePath,
        string rulesFilePath,
        string? schemeFilePath = null
    ) : base(
        new NullLogger<FilesOverrider>(),
        rulesFilePath,
        schemeFilePath
    )
    {
        _targetXmlFilePath = SetTarget(targetXmlFilePath);
        TargetXml.Load(_targetXmlFilePath);
    }

    protected override XmlDocument TargetXml { get; set; } = new();

    public override FilesOverrider Processing()
    {
        foreach (var fromXmlPath in _overrideXmlFilesPaths)
        {
            Logger.LogDebug("Processing {0}", fromXmlPath);
            var overridingXmlDocument = new XmlDocument();
            overridingXmlDocument.Load(fromXmlPath);
            Processing(overridingXmlDocument);
        }

        return this;
    }

    public void Save()
    {
        TargetXml.Save(_targetXmlFilePath);
    }

    public FilesOverrider AddOverride(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Override xml file does not exist: [{filePath}]");
        }

        _overrideXmlFilesPaths.Add(filePath);
        return this;
    }

    public FilesOverrider AddOverride(List<string> filePaths)
    {
        var notExists = filePaths.Where(filePath => !File.Exists(filePath)).ToList();
        if (notExists.Any())
        {
            throw new FileNotFoundException($"Override xml files does not exist: [{string.Join("]; [", notExists)}]");
        }

        _overrideXmlFilesPaths.AddRange(filePaths);
        return this;
    }

    private static string SetTarget(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Target xml file does not exist: [{filePath}]");
        }

        return filePath;
    }
}