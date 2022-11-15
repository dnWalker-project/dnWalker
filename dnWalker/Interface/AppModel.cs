using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Configuration;
using dnWalker.Input;
using dnWalker.Input.Xml;
using dnWalker.TypeSystem;
using dnWalker.Z3;

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

        [MemberNotNull(nameof(_modelParser))]
        private XmlUserModelParser ModelParser
        {
            get
            {
                _modelParser ??= new XmlUserModelParser(DefinitionProvider);
                return _modelParser;
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
            // TODO add configurtion building
            return null;
        }

        public bool Explore(string method, string output)
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

                ExplorationResult explorationResult = explorer.Run(GetMethod(method, definitionProvider));

                // write the output


                return true;
            }
            catch
            {
                return false;
            }
        }

        private static TypeDef GetTypeDefinition(IDefinitionProvider definitionProvider, string fullNameOrName)
        {
            TypeDef td = definitionProvider.GetTypeDefinition(fullNameOrName);
            if (td == null)
            {
                td = definitionProvider.GetTypeDefinition(null, fullNameOrName);
            }
            return td;
        }

        private static MethodDef GetMethod(string methodSpecification, IDefinitionProvider definitionProvider)
        {
            string[] parts = methodSpecification.Split("::");

            Debug.Assert(parts.Length == 2);

            string typeNameOrFullName = parts[0];

            TypeDef td = GetTypeDefinition(definitionProvider, typeNameOrFullName);

            if (td == null)
            {
                // type not found
                return null;
            }

            parts = methodSpecification.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            string methodName = parts[0];

            MethodDef[] methods = td.FindMethods(methodName).ToArray();

            if (methods.Length == 1)
            {
                return methods[0];
            }

            TypeSig[] argTypes = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(fullNameOrName => GetTypeDefinition(definitionProvider, fullNameOrName).ToTypeSig())
                .ToArray();

            return methods.FirstOrDefault(m => m.Parameters.Select(p => p.Type).SequenceEqual(argTypes, TypeEqualityComparer.Instance));
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
