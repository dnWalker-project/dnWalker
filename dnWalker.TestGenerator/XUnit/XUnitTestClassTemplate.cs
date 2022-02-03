﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace dnWalker.TestGenerator.XUnit
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using dnWalker.TypeSystem;
    using dnWalker.Parameters;
    using dnWalker.TestGenerator.Parameters;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    internal partial class XUnitTestClassTemplate : Templates.TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 10 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"

            ExplorationIterationData iterationData = IterationData;
            ExplorationData explorationData = ExplorationData;

            // setup the namespaces:
            HashSet<string> namespaceNames = new HashSet<string>();

            foreach (IParameter p in iterationData.StartingParameterSet.Parameters.Values)
            {
                namespaceNames.Add(p.Type.Namespace);
            }
            
            foreach (IParameter p in iterationData.EndingParameterSet.Parameters.Values)
            {
                namespaceNames.Add(p.Type.Namespace);
            }

            
            #line default
            #line hidden
            
            #line 27 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"
 
            // basic test namespaces

            
            #line default
            #line hidden
            this.Write("using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing Syste" +
                    "m.Text;\r\nusing System.Threading.Tasks;\r\nusing FluentAssertions;\r\nusing Xunit;\r\nu" +
                    "sing Moq;\r\n");
            
            #line 38 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"

            // include namespaces
            foreach (string namespaceName in namespaceNames)
            {

            
            #line default
            #line hidden
            this.Write("using ");
            
            #line 43 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(namespaceName));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 44 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"

            }

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 48 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"

            MethodSignature method = ExplorationData.MethodSignature;
            TypeSignature declaringType = method.DeclaringType;

            
            #line default
            #line hidden
            this.Write("public class ");
            
            #line 52 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(declaringType.Name));
            
            #line default
            #line hidden
            this.Write("_");
            
            #line 52 "C:\Users\hejlb\Documents\CVUT-FEL\MGR\Diplomka\src\dnWalker\dnWalker.TestGenerator\XUnit\XUnitTestClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
