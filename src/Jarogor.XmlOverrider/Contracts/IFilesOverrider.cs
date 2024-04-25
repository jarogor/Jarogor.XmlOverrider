using System.Collections.Generic;

namespace Jarogor.XmlOverrider.Contracts;

public interface IFilesOverrider<out T>
{
    public void Save();
    public T AddOverride(string filePath);
    public T AddOverride(List<string> filePaths);
}