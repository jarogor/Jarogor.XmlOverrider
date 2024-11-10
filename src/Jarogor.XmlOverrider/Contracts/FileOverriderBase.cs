using System.Collections.Generic;
using Jarogor.XmlOverrider.Scheme;
using Microsoft.Extensions.Logging;

namespace Jarogor.XmlOverrider.Contracts;

/// <summary>
///     Base overriding for file
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FileOverriderBase<T> : OverriderBase<T>, IFilesOverrider<T> {
    /// <inheritdoc />
    protected FileOverriderBase(Rules rules, ILogger<T> logger) : base(rules, logger) { }

    /// <inheritdoc />
    public abstract void Save();

    /// <inheritdoc />
    public abstract T AddOverride(string filePath);

    /// <inheritdoc />
    public abstract T AddOverride(List<string> filePaths);
}
