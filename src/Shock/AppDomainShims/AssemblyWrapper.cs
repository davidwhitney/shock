using System.Reflection;

namespace Shock.AppDomainShims
{
    public class AssemblyWrapper : IAssemblyWrapper
    {
        public AssemblyName AssemblyNameGetAssemblyName(string assemblyFile)
        {
            return AssemblyName.GetAssemblyName(assemblyFile);
        }
    }
}
