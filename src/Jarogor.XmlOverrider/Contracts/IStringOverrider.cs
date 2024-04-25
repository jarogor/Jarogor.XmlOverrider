using System.Xml;

namespace Jarogor.XmlOverrider.Contracts;

public interface IStringOverrider<out T>
{
    public T AddOverride(XmlDocument overridingXmlDocument);
}