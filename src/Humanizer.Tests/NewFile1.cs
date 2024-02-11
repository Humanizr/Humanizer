using System.Runtime.CompilerServices;
using VerifyTests;

[ModuleInitializer]
public static void ModuleInitializer()
{
    VerifierSettings.InitializePlugins();
}
#if NET48

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
sealed class ModuleInitializerAttribute : Attribute
{
}
#endif
