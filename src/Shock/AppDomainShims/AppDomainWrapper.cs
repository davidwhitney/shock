using System;
using System.Reflection;

namespace Shock.AppDomainShims
{
    public class AppDomainWrapper : IAppDomainWrapper
    {
        public void CurrentDomainLoad(AssemblyName assemblyName)
        {
            AppDomain.CurrentDomain.Load(assemblyName);
        }

        public Assembly[] CurrentDomainGetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
