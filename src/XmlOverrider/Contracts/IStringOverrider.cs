using System.Xml;

namespace XmlOverrider.Contracts;

public interface IStringOverrider<out T>
{
    public T AddOverride(XmlDocument overridingXmlDocument);
}