using dnWalker.Configuration;
using dnWalker.Input;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal class AppModel
    {
        private readonly IDomain _domain;
        private readonly List<UserModel> _userModels = new List<UserModel>();
        private readonly IConfiguration _configuration;
        private IDefinitionProvider _definitionProvider;

        private AppModel(IConfiguration configuration)
        {
            _domain = Domain.Create();

        }

        public IDefinitionProvider DefinitionProvider
        {
            get
            {
                _definitionProvider ??= new DefinitionProvider(_domain);
                return _definitionProvider;
            }
        }

        public ICollection<UserModel> UserModels
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

            return appModel;
        }
        internal static IConfiguration BuildConfiguration(IEnumerable<string> configurationFiles)
        {
            // TODO add configurtion building
            return null;
        }
    }
}
