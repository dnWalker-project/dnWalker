using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.Linq;
using System.Text;

namespace dnWalker.NativePeers
{
    public class SystemString : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            IDataElement dataElement = null;

            if (method.FullName == "System.Boolean System.String::op_Equality(System.String,System.String)")
            {
                dataElement = new Int4(args[0].CompareTo(args[1]) == 0 ? 1 : 0);
            }

            if (method.FullName == "System.String System.String::Format(System.String,System.Object)")
            {
                var format = (ConstantString)args[0];
                //cur.DynamicArea.Allocations[args[1]];
                var allocatedObject = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)args[1]];
                var value = allocatedObject.Fields[allocatedObject.ValueFieldOffset];
                var arg = ((IConvertible)value).ToString(System.Globalization.CultureInfo.CurrentCulture);
                dataElement = new ConstantString(string.Format(format.Value, arg));
            }

            else if (method.FullName == "System.String System.String::Trim()")
            {
                var s = (ConstantString)args[0];
                dataElement = new ConstantString(s.Value.Trim());
            }

            else if (method.FullName == "System.Int32 System.String::get_Length()")
            {
                var s = (ConstantString)args[0];
                dataElement = new Int4(s.Value.Length);
            }

            else if (method.FullName.StartsWith("System.String System.String::Concat("))
            {
                var sb = new StringBuilder();
                foreach (var arg in args)
                {
                    if (arg is ConstantString cs)
                    {
                        sb.Append(cs.Value);
                        continue;
                    }
                    sb.Append(arg.ToString());
                }
                dataElement = new ConstantString(sb.ToString());
            }

            else if (method.FullName.StartsWith("System.Boolean System.String::StartsWith(System.String"))
            {
                bool result = ((ConstantString)args[0]).Value.StartsWith(((ConstantString)args[1]).Value);
                dataElement = new Int4(result ? 1 : 0);
            }

            else if (method.FullName.StartsWith("System.Boolean System.String::EndsWith(System.String"))
            {
                bool result = ((ConstantString)args[0]).Value.EndsWith(((ConstantString)args[1]).Value);
                dataElement = new Int4(result ? 1 : 0);
            }

            else if (method.FullName.StartsWith("System.Boolean System.String::Contains(System.String"))
            {
                bool result = ((ConstantString)args[0]).Value.Contains(((ConstantString)args[1]).Value);
                dataElement = new Int4(result ? 1 : 0);
            }

            else if (method.FullName.StartsWith("System.Boolean System.String::Substring(System.Int32"))
            {
                string str = ((ConstantString)args[0]).Value;
                int offset = ((INumericElement)args[1]).ToInt4(false).Value;

                int length = (args.Length == 3) ? ((INumericElement)args[2]).ToInt4(false).Value : str.Length - offset;

                str = str.Substring(offset, length);

                dataElement = new ConstantString(str);
            }

            else if (method.FullName == "System.Boolean System.String::IsNullOrEmpty(System.String)")
            {
                throw new System.Exception(method.FullName);
            }

            if (dataElement != null)
            {
                cur.EvalStack.Push(dataElement);
                iieReturnValue = InstructionExecBase.nextRetval;
                return true;
            }

            iieReturnValue = null;
            return false;
        }

        public override bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
        {
            if (methodDef.FullName == "System.Void System.String::.ctor(System.Char[])")
            {
                var arrayRef = (IReferenceType)args[1];
                var theArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];
                if (theArray.Fields.Length > 0)
                {
                    // TODO improve
                    var s = new string(theArray.Fields.Cast<INumericElement>().Select(i => (char)i.ToInt4(true).Value).ToArray());
                    cur.EvalStack.Push(new ConstantString(s));
                    return true;
                }

                var dataElement = new ConstantString();// new string((char)Convert.ChangeType(args[0], typeof(char)));
                cur.EvalStack.Push(dataElement);
                return true;
            }

            return false;
        }
    }
}
