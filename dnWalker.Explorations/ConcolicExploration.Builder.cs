﻿using dnlib.DotNet;

using System;
using System.Collections.Generic;

namespace dnWalker.Explorations
{
    public partial class ConcolicExploration
    {
        public class Builder
        {
            public string? AssemblyName { get; set; }
            public string? AssemblyFileName { get; set; }
            public IMethod? MethodUnderTest { get; set; }
            public string? Solver { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public bool Failed { get; set; }

            public List<ConcolicExplorationIteration.Builder> Iterations { get; } = new List<ConcolicExplorationIteration.Builder>();


            public ConcolicExploration Build()
            {
                if (AssemblyName == null) throw new NullReferenceException("AssemblyName is NULL");
                if (AssemblyFileName == null) throw new NullReferenceException("AssemblyFileName is NULL");
                if (MethodUnderTest == null) throw new NullReferenceException("MethodUnderTest is NULL");
                if (Solver == null) throw new NullReferenceException("Solver is NULL");

                ConcolicExploration exploration = new ConcolicExploration(AssemblyName, AssemblyFileName, MethodUnderTest, Solver, Start, End, Failed);

                foreach (var i in Iterations)
                {
                    i.Exploration = exploration;
                    exploration._iterations.Add(i.Build());
                }



                return exploration;
            }
        }
    }
}
