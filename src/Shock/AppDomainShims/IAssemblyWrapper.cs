using System.Reflection;

namespace Shock.AppDomainShims
{
    public interface IAssemblyWrapper
    {
        AssemblyName AssemblyNameGetAssemblyName(string assemblyFile);
    }
}