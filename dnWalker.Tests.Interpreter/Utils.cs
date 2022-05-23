using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Instructions.Extensions;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dnWalker.Tests.Interpreter
{
    internal static class Utils
    {
        private static Lazy<Dictionary<string, Type>> _lazyTypes = new Lazy<Dictionary<string, Type>>(Init);

        private static Dictionary<string, Type> Init()
        {
            Dictionary<string, Type>? typesDict = new Dictionary<string, Type>();

            IEnumerable<Assembly>? assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => !Regex.IsMatch(t.FullName!, @"^(system|xunit|microsoft)\.|^(system|mscorlib),", RegexOptions.IgnoreCase))
                .Distinct();

            List<Type>? types = assemblies.SelectMany(a => a.GetTypes())
                .Distinct()
                .Where(t => !t.FullName!.StartsWith("System."))
                .ToList();

            foreach (Type? t in types)
            {
                if (typesDict.ContainsKey(t.FullName!))
                {
                    continue;
                }
                typesDict.Add(t.FullName!, t);
            }

            return typesDict;
        }

        public static MethodInfo GetMethodInfo(string methodName)
        {
            var lastDot = methodName.LastIndexOf(".");
            var methodTypeName = methodName.Substring(0, lastDot);

            if (!_lazyTypes.Value.TryGetValue(methodTypeName, out var type))
            {
                throw new Exception($"Type {methodName} not found");
            }

            return type.GetMethod(methodName.Substring(lastDot + 1)) ?? throw new Exception("Method Info not found!");
        }

        public static Explorer GetModelChecker(string methodName, object[] args, IDefinitionProvider definitionProvider, IStatistics? statistics = null, IConfig? config = null, Logger? logger = null)
        {
            return GetModelChecker(methodName, cur => CreateArguments(args, cur), definitionProvider, statistics, config, logger);

            static DataElementList CreateArguments(object[] args, ExplicitActiveState cur)
            {
                DataElementList argList = cur.StorageFactory.CreateList(args.Length);

                for (int i = 0; i < argList.Length; ++i)
                {
                    argList[i] = AsDataElement(args[i]);
                }
            
                return argList;
            }

            static IDataElement AsDataElement(object o)
            {
                switch (o)
                {
                    case bool b: return new Int4(b ? 1 : 0);
                    case char c: return new Int4(c);
                    case byte b: return new Int4(b);
                    case sbyte sb: return new Int4(sb);
                    case short s: return new Int4(s);
                    case ushort us: return new Int4(us);
                    case int i: return new Int4(i);
                    case uint ui: return new UnsignedInt4(ui);
                    case long l: return new Int8(l);
                    case ulong ul: return new UnsignedInt8(ul);
                    case float f: return new Float4(f);
                    case double d: return new Float8(d);
                    case string s: return new ConstantString(s);
                    case IntPtr iptr: return IntPtr.Size == 4 ? new Int4(iptr.ToInt32()) : new Int8(iptr.ToInt64());
                    case UIntPtr uiptr: return UIntPtr.Size == 4 ? new UnsignedInt4(uiptr.ToUInt32()) : new UnsignedInt8(uiptr.ToUInt64());
                    case null: return ObjectReference.Null;
                    default:
                        throw new ArgumentException($"Invalid argument type: {o.GetType().FullName}");
                }
            }
        }

        public static Explorer GetModelChecker(string methodName, Func<ExplicitActiveState, DataElementList> argsProvider, IDefinitionProvider definitionProvider, IStatistics? statistics = null, IConfig? config = null, Logger? logger = null)
        {
            statistics ??= new SimpleStatistics();
            config ??= new Config();
            logger ??= new Logger();

            MethodDef entryPoint = definitionProvider.GetMethodDefinition(methodName) ?? throw new NullReferenceException($"Method {methodName} not found");

            // a bit dirty...
            PathStore pathStore = new PathStore(entryPoint, new ControlFlowGraphProvider());

            var f = new dnWalker.Instructions.ExtendableInstructionFactory().AddStandardExtensions();
            IInstructionExecProvider instructionExecProvider = InstructionExecProvider.Get(config, f);

            ExplicitActiveState cur = new ExplicitActiveState(config, instructionExecProvider, definitionProvider, logger);
            cur.PathStore = pathStore;

            MethodState mainState = new MethodState(entryPoint, argsProvider(cur), cur);

            cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, logger));

            Explorer explorer = new Explorer(cur, statistics, logger, config, pathStore);
            return explorer;
        }
    }
}
