using dnlib.DotNet;

using dnWalker.Symbolic.Variables;

namespace dnWalker.Symbolic
{
    /// <summary>
    /// Helper methods for creating variables.
    /// </summary>
    public static class Variable
    {
        public static IVariable MethodArgument(Parameter parameter)
        {
            return new MethodArgumentVariable(parameter);
        }

        public static IVariable StaticField(IField field)
        {
            return new StaticFieldVariable(field);
        }

        public static IVariable InstanceField(IVariable instance, IField field)
        {
            return new InstanceFieldVariable(instance, field);
        }

        public static IVariable ArrayElement(IVariable array, int index)
        {
            return new ArrayElementVariable(array, index);
        }

        public static IVariable ArrayLength(IVariable array)
        {
            return new ArrayLengthVariable(array);
        }

        public static IVariable MethodResult(IVariable instance, IMethod method, int invocation)
        {
            return new MethodResultVariable(instance, method, invocation);
        }
    }
}
