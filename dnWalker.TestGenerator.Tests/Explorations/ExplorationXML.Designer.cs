﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dnWalker.TestGenerator.Tests.Explorations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExplorationXML {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExplorationXML() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("dnWalker.TestGenerator.Tests.Explorations.ExplorationXML", typeof(ExplorationXML).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Exploration AssemblyName=&quot;{0}&quot; AssemblyFileName=&quot;{1}&quot; MethodName=&quot;{2}&quot; IsStatic=&quot;{3}&quot; /&gt;.
        /// </summary>
        internal static string Exploration_NoIterations_Format {
            get {
                return ResourceManager.GetString("Exploration_NoIterations_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Iteration Number=&quot;{0}&quot;&gt;
        ///  &lt;InputParameters&gt;
        ///    &lt;ParameterStore /&gt;
        ///  &lt;/InputParameters&gt;
        ///&lt;/Iteration&gt;.
        /// </summary>
        internal static string Iteration_NoInputArgs_Format {
            get {
                return ResourceManager.GetString("Iteration_NoInputArgs_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Iteration Number=&quot;{0}&quot; PathConstraint=&quot;(x &gt; {1})&quot;&gt;
        ///  &lt;InputParameters&gt;
        ///    &lt;ParameterStore&gt;
        ///      &lt;PrimitiveValue Type=&quot;System.Double&quot; Name=&quot;{2}&quot;&gt;{3}&lt;/PrimitiveValue&gt;
        ///    &lt;/ParameterStore&gt;
        ///  &lt;/InputParameters&gt;
        ///&lt;/Iteration&gt;.
        /// </summary>
        internal static string Iteration_PrimitiveInputArgs_Format {
            get {
                return ResourceManager.GetString("Iteration_PrimitiveInputArgs_Format", resourceCulture);
            }
        }
    }
}
