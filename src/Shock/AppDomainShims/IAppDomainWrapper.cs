using System.Reflection;

namespace Shock.AppDomainShims
{
    public interface IAppDomainWrapper
    {
        void CurrentDomainLoad(AssemblyName assemblyName);
    }
}