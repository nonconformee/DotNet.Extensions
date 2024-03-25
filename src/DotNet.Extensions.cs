
using System.Reflection;

[assembly: AssemblyVersion("1.0")]
[assembly: AssemblyFileVersion("1.0.0.1"),]
[assembly: AssemblyInformationalVersion("1.0.0-alpha.1"),]

[assembly: AssemblyTitle("nonconformee.DotNet.Extensions")]
[assembly: AssemblyDescription("Little extensions and helpers for .NET")]
[assembly: AssemblyProduct("https://github.com/nonconformee/DotNet.Extensions")]
[assembly: AssemblyCopyright("MIT license")]

[assembly: AssemblyCompany("")]
[assembly: AssemblyTrademark("")]

#if DEBUG
[assembly: AssemblyConfiguration("debug"),]
#elif RELEASE
    [assembly: AssemblyConfiguration("release")]
#else
    #error "DEBUG or RELEASE not specified"
#endif
