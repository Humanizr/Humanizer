#if !(NET5_0_OR_GREATER)

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
internal sealed class IntrinsicAttribute : Attribute
{
}
#endif