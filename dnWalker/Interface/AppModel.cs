using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Configuration;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Input;
using dnWalker.Input.Xml;
using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;
using dnWalker.Z3;

using MMC;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Interface
{
    internal class AppModel : IAppModel
    {
        private readonly IDomain _domain;
        private readonly List<UserModel> _userModels = new List<UserModel>();
        private readonly IConfiguration _configuration;
        private IDefinitionProvider _definitionProvider;

        private XmlUserModelParser _modelParser;
        private XmlExplorationSerializer _explorationSerializer;


        [MemberNotNull(nameof(_modelParser))]
        [MemberNotNull(nameof(_definitionProvider))]
        private XmlUserModelParser ModelParser
        {
            get
            {
                _modelParser ??= new XmlUserModelParser(DefinitionProvider);
                return _modelParser;
            }
        }

        [MemberNotNull(nameof(_explorationSerializer))]
        [MemberNotNull(nameof(_definitionProvider))]
        private XmlExplorationSerializer ExplorationSerializer
        {
            get
            {
                if (_explorationSerializer == null)
                {
                    TypeParser tp = new TypeParser(DefinitionProvider);
                    MethodParser mp = new MethodParser(DefinitionProvider, tp);

                    XmlModelSerializer ms = new XmlModelSerializer(tp, mp);
                    _explorationSerializer = new XmlExplorationSerializer(ms);
                }
                return _explorationSerializer;
            }
        }

        private AppModel(IConfiguration configuration)
        {
            _domain = dnWalker.TypeSystem.Domain.Create();

        }


        [MemberNotNull(nameof(_definitionProvider))]
        public IDefinitionProvider DefinitionProvider
        {
            get
            {
                _definitionProvider ??= new DefinitionProvider(_domain);
                return _definitionProvider;
            }
        }

        public IReadOnlyList<UserModel> UserModels
        {
            get
            {
                return _userModels;
            }
        }

        public IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public static AppModel Create(Options options) 
        {
            AppModel appModel = new AppModel(BuildConfiguration(options.ConfigurationFiles));

            if (!string.IsNullOrWhiteSpace(options.Assembly))
            { 
                appModel._domain.Load(options.Assembly);
            }

            return appModel;
        }
        internal static IConfiguration BuildConfiguration(IEnumerable<string> configurationFiles)
        {
            // WIP: right now the defaults are not overwritten by other configuration providers

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.InitializeDefaults();

            foreach (string cfgFile in configurationFiles)
            {
                string extension = System.IO.Path.GetExtension(cfgFile);
                switch (extension)
                {
                    case ".json":
                        builder.AddJsonFile(cfgFile);
                        break;

                    case ".ini":
                        builder.AddIniFile(cfgFile);
                        break;
                }
            }

            if (!builder.HasValue("Strategy"))
            {
                builder.SetValue("Strategy", "dnWalker.Concolic.Traversal.AllPathsCoverage");
            }

            // TODO add configurtion building
            return builder.Build();
        }

        public bool Explore(string methodSpecification, string output)
        {

            try
            {
                IDefinitionProvider definitionProvider = DefinitionProvider;

                ConcolicExplorer explorer = new ConcolicExplorer
                    (
                        definitionProvider,
                        _configuration,
                        new MMC.Logger(),
                        new Z3Solver()
                    );

                MethodDef md = definitionProvider.GetMethodDefinitionPartial(methodSpecification);

                ExplorationResult explorationResult = explorer.Run(md);

                // write the output
                ConcolicExploration data = explorationResult.ToExplorationData().Build();
                XmlExplorationSerializer serializer = ExplorationSerializer;

                output ??= $"{md.Name}_dnWalkerExploration.xml";
                serializer.ToXml(data).Save(output);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LoadAssembly(string assemblyFile)
        {
            try
            {
                return _domain.Load(assemblyFile);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Load Assembly Error: '{e}'");
                return false;
            }
        }

        public bool LoadModels(string modelsFile)
        {
            try
            {
                XElement xml = XElement.Load(modelsFile);

                XmlUserModelParser parser = ModelParser;
                _userModels.AddRange(parser.ParseModelCollection(xml));

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Load Models Error: '{e}'");
                return false;
            }
        }
    }
}
