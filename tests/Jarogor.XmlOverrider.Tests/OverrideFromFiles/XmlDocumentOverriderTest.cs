﻿using System.Xml;

using Jarogor.XmlOverrider.Overrider;

using NUnit.Framework;

namespace Jarogor.XmlOverrider.Tests.OverrideFromFiles;

[TestFixture]
public class XmlDocumentOverriderTest : TestBase
{
    [Test]
    public void OverridingFromXmlDocumentSuccess()
    {
        XmlDocumentOverrider? overrider = new(Rules, LoadXml(TargetXmlFilePath));

        foreach (string? file in FromXmlFiles)
        {
            overrider.AddOverride(LoadXml(file));
        }

        XmlDocument? actual = overrider.Processing().Get();
        Assert.That(actual.OuterXml, Is.EqualTo(ExpectedXml));
    }
}