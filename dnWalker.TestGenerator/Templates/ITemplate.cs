﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public interface ITemplate
    {
        IEnumerable<string> Namespaces { get; }
    }
}
