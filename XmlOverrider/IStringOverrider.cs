using System.Xml;

namespace XmlOverrider;

public interface IStringOverrider<out T>
{
    public T AddOverride(XmlDocument overridingXmlDocument);
}