using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.TypeSystem;
using dnWalker.Z3;

using System;
using System.Diagnostics;
using System.Linq;

namespace dnWalker.Interface.Commands
{
    internal class ExploreCommand : ICommand
    {
        private readonly string _outputFile;
        private readonly string _methodSpecification;

        public ExploreCommand(string methodSpecification, string outputFile)
        {
            _methodSpecification = methodSpecification;
            _outputFile = outputFile;
        }

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"Running Explore command: '{_methodSpecification}'");

            try
            {
                ConcolicExplorer explorer = new ConcolicExplorer
                    (
                        appModel.DefinitionProvider,
                        appModel.Configuration, 
                        new MMC.Logger(),
                        new Z3Solver()
                    );

                ExplorationResult explorationResult = explorer.Run(GetMethod(_methodSpecification, appModel.DefinitionProvider));

                // write the output


                return CommandResult.Success;
            }
            catch
            {
                return CommandResult.BreakFail(-1);
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
    }
}