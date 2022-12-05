﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal static class Runner
    {
        public static int Run(Options options)
        {
            return GetRunner(options).Run();

        }

        public static IRunner GetRunner(Options options)
        {
            if (options.Batch)
            {
                return new BatchModeRunner(options);
            }
            else
            {
                return new InteractiveRunner(options);
            }
        }
    }
}
