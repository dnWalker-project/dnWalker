using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.TestWriter.TestWriters
{
    public class CsProjWriter : IDisposable
    {
        private static readonly XmlWriterSettings _settings = new XmlWriterSettings()
        {
            Indent = true,
            OmitXmlDeclaration = true,
            IndentChars = "    "
        };

        private readonly XmlWriter _output;

        public CsProjWriter(TextWriter target) 
        {
            _output = XmlWriter.Create(target, _settings);
        }

        public void Write(TestProject p)
        {
            _output.WriteStartElement(XmlTokens.Project);
            _output.WriteAttributeString(XmlTokens.Sdk, XmlTokens.SdkValue);
            {

                {
                    _output.WriteStartElement(XmlTokens.PropertyGroup);

                    // necessary properties, must be used
                    if (!p.Properties.TryGetValue(XmlTokens.TargetFramework, out _))
                    {
                        _output.WriteElementString(XmlTokens.TargetFramework, XmlTokens.FallbackTargetFrameworkValue);
                    }
                    if (!p.Properties.TryGetValue(XmlTokens.IsPackable, out _))
                    {
                        _output.WriteElementString(XmlTokens.IsPackable, XmlTokens.FallbackIsPackableValue);
                    }

                    foreach ((string pName, string pValue) in p.Properties)
                    {
                        _output.WriteElementString(pName, pValue);
                    }

                    _output.WriteEndElement();
                }

                if (p.Packages.Count > 0) 
                {

                    _output.WriteStartElement(XmlTokens.ItemGroup);

                    foreach (PackageReference pr in p.Packages)
                    {
                        _output.WriteStartElement(XmlTokens.PackageReference);

                        _output.WriteAttributeString(XmlTokens.Include, pr.Name);
                        _output.WriteAttributeString(XmlTokens.Version, pr.Version?.ToString() ?? "*");

                        if (pr.IncludeAssets.Count > 0) 
                        {
                            _output.WriteElementString(XmlTokens.IncludeAssets, string.Join("; ", pr.IncludeAssets));
                        }

                        if (pr.ExcludeAssets.Count > 0)
                        {
                            _output.WriteElementString(XmlTokens.ExcludeAssets, string.Join("; ", pr.ExcludeAssets));
                        }

                        if (pr.PrivateAssets.Count > 0)
                        {
                            _output.WriteElementString(XmlTokens.PrivateAssets, string.Join("; ", pr.PrivateAssets));
                        }

                        _output.WriteEndElement();
                    }

                    _output.WriteEndElement();
                }

            }
            _output.WriteEndElement();
        }

        public void Dispose()
        {
            _output.Close();
        }
    }
}
