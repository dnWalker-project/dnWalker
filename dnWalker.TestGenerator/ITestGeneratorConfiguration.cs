﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public interface ITestGeneratorConfiguration
    {
        bool PreferLiteralsOverVariables { get; set; }
    }
}
