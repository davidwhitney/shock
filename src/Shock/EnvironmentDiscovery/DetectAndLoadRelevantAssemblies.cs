using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shock.EnvironmentDiscovery
{
    public class DetectAndLoadRelevantAssemblies : IDetectAndLoadRelevantAssemblies
    {
        public AppDomain LoadEnvironmentFrom(string[] args)
        {
            var loadThese = new List<string>();

            if(args.FirstOrDefault(x => x.EndsWith(".dll")) != null)
            {
                loadThese.Add(args.FirstOrDefault(x => x.EndsWith(".dll")));
            }
            
            loadThese.RemoveAll(x => x == null);

            loadThese.ForEach(assemblyFile =>
            {
                var assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
                AppDomain.CurrentDomain.Load(assemblyName);
            });

            return AppDomain.CurrentDomain;
        }
    }
}