﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IModelVisitable
    {
        void Accept(IModelVisitor visitor);
    }
}
