using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestModels
{
    public class PackageVersion
    {
        public Version? Version { get; set; }
        public string? Channel { get; set; }

        public static PackageVersion Parse(string versionString)
        {
            int channelStart = versionString.IndexOf('-');
            string? channel = null; 
            if (channelStart >= 0) 
            {
                channel = versionString.Substring(channelStart + 1);
                versionString = versionString.Substring(0, channelStart);
            }

            Version? v = versionString == "*" ? null : Version.Parse(versionString);
            
            return new PackageVersion() { Version = v, Channel = channel };
        }

        public override string ToString()
        {
            if (Version == null && Channel == null)
            {
                return "*";
            }
            else if (Version != null && Channel == null) 
            {
                return Version!.ToString();
            }
            else if (Version == null && Channel != null)
            {
                return "*-" + Channel;
            }
            else // if (Version != null && Channel != null)
            {
                return $"{Version}-{Channel}";
            }
        }
    }
}
