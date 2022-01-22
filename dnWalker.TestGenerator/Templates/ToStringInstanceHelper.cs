using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class ToStringInstanceHelper
    {
        private static readonly Dictionary<Type, MethodInfo> _convertorCache = new Dictionary<Type, MethodInfo>();
        private static readonly Type[] _convertorParamterCache = new Type[] { typeof(IFormatProvider) };
        private readonly object[] _convertorParameters = new object[1] { CultureInfo.InvariantCulture };

        public IFormatProvider FormatProvider
        {
            get 
            { 
                return (IFormatProvider) _convertorParameters[0]; 
            }
            set
            {
                if (value != null)
                {
                    _convertorParameters[0] = value;
                }
            }
        }

        public string ToStringWithCulture(object objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException(nameof(objectToConvert));
            }

            Type t = objectToConvert.GetType();
            MethodInfo? method = t.GetMethod("ToString", _convertorParamterCache);
            if (method == null)
            {
                return objectToConvert.ToString() ?? string.Empty;
            }
            else
            {
                return ((string?)method.Invoke(objectToConvert, _convertorParameters)) ?? string.Empty;
            }
        }
    }
}
