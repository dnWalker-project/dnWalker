﻿using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.TestWriter.TestWriters
{
    internal class CsProjWriter : IDisposable
    {
        private static readonly XmlWriterSettings _settings = new XmlWriterSettings()
        {
            Indent = true,
            OmitXmlDeclaration = true,
        };

        private readonly XmlWriter _output;

        public CsProjWriter(string targetFile) 
        {
            _output = XmlWriter.Create(targetFile, _settings);
        }

        public void Write(TestProject p)
        {
            _output.WriteStartElement(XmlTokens.Project);
            _output.WriteAttributeString(XmlTokens.Sdk, XmlTokens.SdkValue);
            {

                _output.WriteStartElement(XmlTokens.PropertyGroup);

                // necessary properties, must be used
                if (!p.Properties.TryGetValue(XmlTokens.TargetFramework, out _))
                {
                    _output.WriteElementString(XmlTokens.TargetFramework, v);
                }
                if (!p.Properties.TryGetValue(XmlTokens.IsPackable, out _))
                {
                    _output.WriteElementString(XmlTokens.IsPackable, v);
                }

                foreach ((string pName, string pValue) in p.Properties)
                {
                    _output.WriteElementString(pName, pValue);
                }

                _output.WriteEndElement();

                _output.WriteStartElement(XmlTokens.ItemGroup);

                foreach (PackageReference pr in p.Packages)
                {
                    _output.WriteStartElement(XmlTokens.PackageReference);

                    _output.WriteAttributeString(XmlTokens.Include, pr.Name);
                    _output.WriteAttributeString(XmlTokens.Version, pr.Version);

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
            _output.WriteEndElement();
        }

        public void Dispose()
        {
            _output.Close();
        }
    }
}
