using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.AppDomainShims;
using Shock.Logging;

namespace Shock.EnvironmentDiscovery
{
    public class DetectAndLoadRelevantAssemblies : IDetectAndLoadRelevantAssemblies
    {
        private readonly IAppDomainWrapper _appDomain;
        private readonly IAssemblyWrapper _assembly;
        private readonly IFileSystemWrapper _fs;
        private readonly IOutput _output;

        public List<AssemblyName> LoadedAssemblies { get; } = new List<AssemblyName>();

        public DetectAndLoadRelevantAssemblies(IAppDomainWrapper appDomain, IAssemblyWrapper assembly, IFileSystemWrapper fs, IOutput output)
        {
            _appDomain = appDomain;
            _assembly = assembly;
            _fs = fs;
            _output = output;
        }

        public void LoadEnvironmentFrom(string[] args)
        {
            var loadThese = new List<string>();

            if(args.FirstOrDefault(x => x.EndsWith(".dll")) != null)
            {
                loadThese.Add(args.FirstOrDefault(x => x.EndsWith(".dll")));
            }

            loadThese.RemoveAll(x => x == null);

            if (!loadThese.Any())
            {
                var currentDirFiles = _fs.DirectoryGetFiles();
                loadThese.AddRange(currentDirFiles.Where(x => x.EndsWith(".dll")));
            }

            loadThese.ForEach(TryLoadIntoAppDomain);
        }

        private void TryLoadIntoAppDomain(string assemblyFile)
        {
            try
            {
                var assemblyName = _assembly.AssemblyNameGetAssemblyName(assemblyFile);
                _appDomain.CurrentDomainLoad(assemblyName);
                LoadedAssemblies.Add(assemblyName);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Skipped loading '{assemblyFile}' because '{ex.Message}'");
            }
        }
    }
}