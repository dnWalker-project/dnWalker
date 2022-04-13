using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Parameters
{
    public class LDELEMA : LDELEM
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Ldelema };

        public override IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        // does the same as LDELEM, e.i. lazily initialize the data element whose address will be returned...
    }
}
