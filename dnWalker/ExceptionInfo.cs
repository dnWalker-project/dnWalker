using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC.State
{
    public class ExceptionInfo
    {
        public ExceptionInfo(ITypeDefOrRef type, String message)
        {
            Type = type;
            Message = message;
        }


        public ITypeDefOrRef Type { get; }
        public string Message { get; }
    }
}
