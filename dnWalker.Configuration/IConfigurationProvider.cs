﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public interface IConfigurationProvider
    {
        bool TryGetValue(string key, Type type, out object? value);
    }
}