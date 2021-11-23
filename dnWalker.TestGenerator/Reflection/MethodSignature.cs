using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Reflection
{
    public class MethodSignature
    {
        private MethodInfo _methodInfo;
        private ParameterInfo[] _parameters;

        public MethodSignature(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            _parameters = methodInfo.GetParameters();
        }


        public Type ReturnType
        {
            get
            {
                return _methodInfo.ReturnType;
            }
        }

        public Type DeclaringType
        {
            get
            {
                return _methodInfo.DeclaringType ?? throw new Exception("DeclaringType should not be null.");
            }
        }

        public ParameterInfo[] Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public MethodInfo MethodInfo
        {
            get
            {
                return _methodInfo;
            }
        }

        public static MethodInfo GetMethodInfo(string signature)
        {
            return FromString(signature).MethodInfo;
        }

        public static MethodSignature FromString(string signature)
        {
            static T CheckNotNull<T>(T? value) where T : class
            {
                if (value == null) throw new NullReferenceException();

                return value;
            }

            // signature format:
            // RETURN_TYPE DECLARING.NAMESPACE.TYPE::METHOD_NAME(PARAMETER.NAMESPACE.TYPE ... )
            string[] signatureParts = signature.Split(new char[] { ' ', ':', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            //string returnType = signatureParts[0];
            string declaringNamespaceAndType = signatureParts[1];
            string methodName = signatureParts[2];

            // !! it is expected that all assemblies were loaded correctly into current AppDomain !!
            Type declaringType = CheckNotNull(AppDomain.CurrentDomain.GetType(declaringNamespaceAndType));

            Type[] parameterTypes;
            if (signatureParts.Length > 3)
            {
                // there are some parameters
                parameterTypes = Enumerable.Range(3, signatureParts.Length - 3).Select(i => CheckNotNull(AppDomain.CurrentDomain.GetType(signatureParts[i]))).ToArray();
            }
            else
            {
                parameterTypes = Type.EmptyTypes;
            }

            MethodInfo methodInfo = CheckNotNull(declaringType.GetMethod(methodName, parameterTypes));

            return new MethodSignature(methodInfo);

        }

        public override string ToString()
        {
            string returnTypeString = ReturnType.FullName!;
            string declaringTypeString = DeclaringType.FullName!;
            string methodName = _methodInfo.Name;
            IEnumerable<string> parameteTypeStrings = Parameters.Select(p => p.ParameterType.FullName!);

            return $"{returnTypeString} {declaringTypeString}::{methodName}({string.Join(',', parameteTypeStrings)})";
        }
    }
}
