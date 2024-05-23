﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Logging;
using XmlOverrider.Contracts;
using XmlOverrider.Scheme;

namespace XmlOverrider.Overrider;

/// <summary>
/// Overrides for files
/// </summary>
public sealed class FilesOverrider : OverriderBase<FilesOverrider>, IFilesOverrider<FilesOverrider>
{
    private readonly string _targetXmlFilePath;
    private readonly List<string> _overrideXmlFilesPaths = new();

    /// <inheritdoc />
    public FilesOverrider(Rules rules, string targetXmlFilePath, ILogger<FilesOverrider>? logger = null)
        : base(rules, logger)
    {
        _targetXmlFilePath = SetTarget(targetXmlFilePath);
        TargetXml.Load(_targetXmlFilePath);
    }

    /// <inheritdoc />
    protected override XmlDocument TargetXml { get; set; } = new();

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Save()
    {
        TargetXml.Save(_targetXmlFilePath);
    }

    /// <inheritdoc />
    public FilesOverrider AddOverride(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Override xml file does not exist: [{filePath}]");
        }

        _overrideXmlFilesPaths.Add(filePath);
        return this;
    }

    /// <inheritdoc />
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