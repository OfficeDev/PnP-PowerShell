using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if SP2013
[assembly: AssemblyTitle("PnP.PowerShell.SP2013.Commands")]
#elif SP2016
[assembly: AssemblyTitle("PnP.PowerShell.SP2016.Commands")]
#elif SP2019
[assembly: AssemblyTitle("PnP.PowerShell.SP2019.Commands")]
#else
[assembly: AssemblyTitle("PnP.PowerShell.Online.Commands")]
#endif
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
#if SP2013
[assembly: AssemblyProduct("PnP.PowerShell.SP2013.Commands")]
#elif SP2016
[assembly: AssemblyProduct("PnP.PowerShell.SP2016.Commands")]
#elif SP2019
[assembly: AssemblyProduct("PnP.PowerShell.SP2019.Commands")]
#elif PNPPSCORE
[assembly: AssemblyProduct("PnP.PowerShell.Online.Commands")]
#else
[assembly: AssemblyProduct("PnP.PowerShell.Online.Commands")]
#endif
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2eebc303-84d4-4dbb-96de-8aaa75248120")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if !PNPPSCORE
[assembly: AssemblyVersion("3.24.2008.0")]
[assembly: AssemblyFileVersion("3.24.2008.0")]
#else
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]
#endif
[assembly: InternalsVisibleTo("PnP.PowerShell.Tests")]