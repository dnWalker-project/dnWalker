﻿using dnlib.DotNet;

using dnWalker.TypeSystem;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using dnWalker.Concolic.Traversal;
using dnWalker.Traversal;

namespace dnWalker.Symbolic
{
    public static class ExplicitActiveStateExtensions
    {
        private const string SymbolicContextAttribute = "symbolic-context";

        public static void Initialize(this ExplicitActiveState cur, IModel inputModel)
        {
            SymbolicContext context = SymbolicContext.Create(inputModel, cur);
            Path currentPath = cur.PathStore.CurrentPath;
            if (currentPath is ConcolicPath concolicPath)
            {
                concolicPath.SymbolicContext = context;
            }
            else
            {
                currentPath.SetPathAttribute(SymbolicContextAttribute, context);
            }
            
        }

        public static ExpressionFactory GetExpressionFactory(this ExplicitActiveState cur)
        {
            if (!cur.Services.TryGetService(out ExpressionFactory factory))
            {
                factory = new CustomModuleExpressionFactory(cur.DefinitionProvider.Context.MainModule);
                cur.Services.RegisterService<ExpressionFactory>(factory);
            }

            Debug.Assert(factory != null);
            return factory;
        }

        public static bool TryGetSymbolicContext(this ExplicitActiveState cur, [NotNullWhen(true)]out SymbolicContext symbolicContext)
        {
            Path currentPath = cur.PathStore.CurrentPath;
            if (currentPath is ConcolicPath concolicPath)
            {
                symbolicContext = concolicPath.SymbolicContext;
                return symbolicContext != null;
            }
            else
            {
                return cur.PathStore.CurrentPath.TryGetPathAttribute(SymbolicContextAttribute, out symbolicContext);
            }
        }
    }
}

