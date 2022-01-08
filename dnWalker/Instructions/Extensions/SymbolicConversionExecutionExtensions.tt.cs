using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{

	public static partial class Extensions
	{
		public static ExtendableInstructionFactory AddSymbolicConversionExecution(this ExtendableInstructionFactory factory)
		{

			factory.RegisterExtension(new CONV_I_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_I1_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_I2_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_I4_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_I8_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I1_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I1_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I2_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I2_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I4_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I4_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I8_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_I8_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U1_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U1_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U2_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U2_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U4_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U4_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U8_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_OVF_U8_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_R_UN_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_R4_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_R8_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_U_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_U1_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_U2_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_U4_SymbolicExecutionExtension());
			factory.RegisterExtension(new CONV_U8_SymbolicExecutionExtension());
			return factory;
		}
	}

	public abstract class SymbolicConversionExtensionBase : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
	{

        public abstract IEnumerable<Type> SupportedInstructions
        {
            get;
        }

		public abstract Type ResultType 
		{
			get; 
		}

		private INumericElement _operand;


        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            IDataElement toPop = cur.EvalStack.Peek();
            _operand = (toPop is IManagedPointer) ? (toPop as IManagedPointer).ToInt4() : (INumericElement)toPop;
        }

        public void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            IDataElement target = cur.EvalStack.Peek();

            if (_operand.TryGetExpression(cur, out Expression expression))
            {
                Type outType = ResultType;
                if (expression.Type == outType)
                {
                    target.SetExpression(expression, cur);
                }
                else
                {
                    target.SetExpression(instruction.CheckOverflow ? Expression.ConvertChecked(expression, outType) : Expression.Convert(expression, outType), cur);
                }
            }
        }
	}
	public class CONV_I_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_I)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_I1_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_I1)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_I2_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_I2)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_I4_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_I4)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_I8_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_I8)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I1_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I1)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I1_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I1_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I2_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I2)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I2_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I2_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I4_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I4)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I4_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I4_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I8_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I8)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_I8_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_I8_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U1_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U1)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U1_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U1_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U2_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U2)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U2_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U2_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U4_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U4)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U4_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U4_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U8_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U8)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_OVF_U8_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_OVF_U8_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_R_UN_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_R_UN)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(double);
			}
		}
	}
	public class CONV_R4_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_R4)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(double);
			}
		}
	}
	public class CONV_R8_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_R8)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(double);
			}
		}
	}
	public class CONV_U_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_U)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_U1_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_U1)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_U2_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_U2)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_U4_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_U4)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
	public class CONV_U8_SymbolicExecutionExtension : SymbolicConversionExtensionBase
	{
		private static readonly Type[] _instructions = new Type[]
		{
			 typeof(CONV_U8)
		};

		public override IEnumerable<Type> SupportedInstructions
        {
            get
			{
				return _instructions;
			}
        }

		public override Type ResultType
		{
			get
			{
				return typeof(int);
			}
		}
	}
}

