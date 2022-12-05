using dnlib.DotNet;

using dnWalker.TestWriter.Utils;

namespace dnWalker.TestWriter.Generators.Arrange
{
    internal class SimpleArrangePrimitives : IArrangePrimitives
    {
        const string PublicInstanceFlags = "System.Reflection.BindingFlags.Public";
        const string NonPublicInstanceFlags = "System.Reflection.BindingFlags.NonPublic";

        const string PublicStaticFlags = "System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static";
        const string NonPublicStaticFlags = "System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static";

        public bool TryWriteArrangeCreateInstance(ITestContext testContext, IWriter output, string symbol)
        {
            // can arrange only if members to initialize are fields
            SymbolContext symbolContext = testContext.SymbolMapping[symbol];
            if (symbolContext.MembersToArrange.OfType<IMethod>().Any())
            {
                return false;
            }

            // create the new instance
            TypeSig type = symbolContext.Type ?? throw new InvalidOperationException("The symbol context is not initialized!!");

            if (type.IsArray || 
                type.IsSZArray) 
            {
                // Type[] symbol = new Type[length];
                string eTypeName = type.Next.GetNameOrAlias();
                output.WriteLine($"{eTypeName}[] {symbol} = new {eTypeName}[{symbolContext.Length}];");
            }
            else
            {
                // Type symbol = new Type();
                string typeName = $"{type.GetNameOrAlias()}";
                output.WriteLine($"{typeName} {symbol} = new {typeName}();");
            }

            return true;
        }

        public bool TryWriteArrangeInitializeField(ITestContext testContext, IWriter output, string symbol, IField field, string literal)
        {
            FieldDef fd = field.ResolveFieldDefThrow();

            // assembly => internal, should it be allowed? do we expect the tested assembly to enable internals visible to?
            if (fd.IsPublic || 
                fd.IsAssembly || 
                fd.IsFamilyOrAssembly)
            {
                if (fd.IsInitOnly)
                {
                    WriteReflectionSetField(testContext, output, symbol, fd, literal);
                }
                else
                {
                    WriteAssignmentSetField(testContext, output, symbol, fd, literal);
                }
            }
            else
            {
                WriteReflectionSetField(testContext, output, symbol, fd, literal);
            }

            return true;
        }

        private void WriteAssignmentSetField(ITestContext testContext, IWriter output, string symbol, FieldDef field, string literal)
        {
            // symbol.field = literal
            output.WriteLine($"{symbol}.{field.Name} = {literal};");
        }

        private void WriteReflectionSetField(ITestContext testContext, IWriter output, string symbol, FieldDef field, string literal)
        {
            // get fieldinfo
            //typeof(type).GetField(field.Name, System.Reflection.BindingFlags. | System.Reflection.BindingFlags.Public).SetValue(symbol, literal);
            string bindingFlags = (field.IsPublic, field.IsStatic) switch
            {
                (true, false) => PublicInstanceFlags,
                (false, false) => NonPublicInstanceFlags,
                (true, true) => PublicStaticFlags,
                (false, true) => NonPublicStaticFlags,
            };

            string typeName = field.DeclaringType!.ToTypeSig().GetNameOrAlias();
            output.WriteLine($"typeof({typeName}).GetField(\"{field.Name}\", {bindingFlags}).SetValue({symbol}, {literal});");
        }

        public bool TryWriteArrangeInitializeMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, params string[] literals)
        {
            return false;
        }

        public bool TryWriteArrangeInitializeArrayElement(ITestContext testContext, IWriter output, string symbol, int index, string literal)
        {
            // symbol[index] = literal;
            output.WriteLine($"{symbol}[{index}] = {literal};");
            return true;
        }

        public bool TryWriteArrangeInitializeStaticField(ITestContext testContext, IWriter output, IField field, string literal)
        {
            FieldDef fd = field.ResolveFieldDefThrow();
            if (fd.IsInitOnly)
            {
                return false;
            }
            if (fd.IsPublic)
            {
                WriteAssignmentSetStaticField(testContext, output, fd, literal); 
            }
            else
            {
                WriteReflectionSetField(testContext, output, "null", fd, literal);
            }

            return true;
        }

        private void WriteAssignmentSetStaticField(ITestContext testContext, IWriter output, FieldDef fd, string literal)
        {
            TypeDef td = fd.DeclaringType;
            // typename.fieldName = literal
            output.WriteLine($"{td.ToTypeSig().GetNameOrAlias()}.{fd.Name} = {literal};");
        }

        public bool TryWriteArrangeInitializeStaticMethod(ITestContext testContext, IWriter output, IMethod method, params string[] literals)
        {
            return false;
        }

        public IEnumerable<string> Namespaces
        {
            get
            {
                return Array.Empty<string>();
            }
        }
    }
}