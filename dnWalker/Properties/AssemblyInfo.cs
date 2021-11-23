using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties

/*[assembly: AssemblyTitle ("MoonWalker")]
[assembly: AssemblyDescription ("Model Checker for CIL bytecode programs")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyProduct ("MoonWalker")]
[assembly: AssemblyVersion ("1.0.1.0")]
[assembly: AssemblyCopyright("(C) 2008, University of Twente - Formal Methods and Tools group")]
[assembly: AssemblyCulture ("")]*/

/*[assembly: CLSCompliant (false)]
[assembly: ComVisible (false)]*/

// [assembly: AssemblyKeyFile("MoonWalkerKey.snk")]
// [assembly: AssemblyCompanyAttribute("University of Twente")]




// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("dnWalker.Tests")]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("2aa75e3c-64a5-46ee-9fef-5bb1253849aa")]