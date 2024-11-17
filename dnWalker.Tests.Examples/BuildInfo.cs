using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples
{
    public class BuildInfo : IXunitSerializable
    {
        public static readonly string[] Configurations = 
        {
            "Debug", 
            "Release",
        };

        public static readonly string[] Targets = 
        { 
            "net8.0",
        };

        private string? _configuration;
        private string? _target;

        public string Configuration => _configuration ?? throw new InvalidOperationException("Not Initialized.");
        public string Target => _target ?? throw new InvalidOperationException("Not Initialized.");

        [Obsolete("Used only by the deserializer!")]
        public BuildInfo()
        {

        }

        public BuildInfo(string configuration, string target)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public bool IsRelease()
        {
            return _configuration == "Release";
        }

        public bool IsDebug()
        {
            return _configuration == "Debug";
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _configuration = info.GetValue<string>(nameof(Configuration));
            _target = info.GetValue<string>(nameof(Target));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Configuration), _configuration);
            info.AddValue(nameof(Target), _target);
        }

        private static BuildInfo[]? _infos = null;

        public static IReadOnlyList<BuildInfo> GetBuildInfos()
        {
            if (_infos != null) return _infos;

            _infos = new BuildInfo[Configurations.Length * Targets.Length];

            int i = 0;
            foreach (string cfg in Configurations)
            {
                foreach (string trg in Targets)
                {
                    _infos[i++] = new BuildInfo(cfg, trg);
                }
            }

            return _infos;
        }
    }
}
