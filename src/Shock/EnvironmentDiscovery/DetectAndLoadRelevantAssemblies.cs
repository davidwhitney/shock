using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.AppDomainShims;

namespace Shock.EnvironmentDiscovery
{
    public class DetectAndLoadRelevantAssemblies : IDetectAndLoadRelevantAssemblies
    {
        private readonly IAppDomainWrapper _appDomain;
        private readonly IAssemblyWrapper _assembly;

        public List<AssemblyName> LoadedAssemblies { get; private set; } = new List<AssemblyName>();

        public DetectAndLoadRelevantAssemblies(IAppDomainWrapper appDomain, IAssemblyWrapper assembly)
        {
            _appDomain = appDomain;
            _assembly = assembly;
        }

        public void LoadEnvironmentFrom(string[] args)
        {
            var loadThese = new List<string>();

            if(args.FirstOrDefault(x => x.EndsWith(".dll")) != null)
            {
                loadThese.Add(args.FirstOrDefault(x => x.EndsWith(".dll")));
            }
            
            loadThese.RemoveAll(x => x == null);

            loadThese.ForEach(assemblyFile =>
            {
                var assemblyName = _assembly.AssemblyNameGetAssemblyName(assemblyFile);
                _appDomain.CurrentDomainLoad(assemblyName);
                LoadedAssemblies.Add(assemblyName);
            });
        }
    }
}