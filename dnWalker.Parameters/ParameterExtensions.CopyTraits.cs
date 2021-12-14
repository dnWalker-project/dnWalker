using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        public static void TransferTraitsInto(this IItemOwnerParameter source, IItemOwnerParameter target)
        {
            for (int i = 0; i < target.GetLength(); ++i)
            {
                if (source.TryGetItem(i, out var item))
                {
                    source.ClearItem(i);

                    if (!target.TryGetItem(i, out _))
                    {
                        target.SetItem(i, item);
                    }
                }
            }
        }

        public static void TransferTraitsInto(this IMethodResolverParameter source, IMethodResolverParameter target)
        {
            foreach (KeyValuePair<MethodSignature, IParameter?[]> resultsInfo in source.GetMethodResults())
            {
                MethodSignature signature = resultsInfo.Key;
                IParameter?[] results = resultsInfo.Value;

                for (int i = 0; i < results.Length; ++i)
                {
                    source.ClearMethodResult(signature, i);

                    if (results[i] != null &&
                        !target.TryGetMethodResult(signature, i, out _))
                    {
                        target.SetMethodResult(signature, i, results[i]);
                    }
                }
            }
        }

        public static void TransferTraitsInto(this IFieldOwnerParameter source, IFieldOwnerParameter target)
        {
            foreach (KeyValuePair<string, IParameter> fieldInfos in source.GetFields())
            {
                string fieldName = fieldInfos.Key;
                IParameter value = fieldInfos.Value;

                source.ClearField(fieldName);

                if (!target.TryGetField(fieldName, out _))
                {
                    target.SetField(fieldName, value);
                }
            }
        }
    }
}
