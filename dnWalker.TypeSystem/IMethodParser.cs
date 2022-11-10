﻿using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IMethodParser
    {
        IMethod Parse(ReadOnlySpan<char> span);
    }
}
