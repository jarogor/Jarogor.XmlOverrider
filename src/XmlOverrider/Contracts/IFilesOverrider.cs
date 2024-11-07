using System.Collections.Generic;

namespace XmlOverrider.Contracts;

/// <summary>
///     Overrides for files
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFilesOverrider<out T> {
    /// <summary>
    ///     Saving a file
    /// </summary>
    public void Save();

    /// <summary>
    ///     Adding an override file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public T AddOverride(string filePath);

    /// <summary>
    ///     Adding an override files list
    /// </summary>
    /// <param name="filePaths"></param>
    /// <returns></returns>
    public T AddOverride(List<string> filePaths);
}
