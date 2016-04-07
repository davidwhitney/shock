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

        private readonly List<string> _loadedPaths = new List<string>();
        public List<AssemblyName> LoadedAssemblies { get; } = new List<AssemblyName>();
        public string ActiveAppConfiguration => AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

        public DetectAndLoadRelevantAssemblies(IAppDomainWrapper appDomain, IAssemblyWrapper assembly,
            IFileSystemWrapper fs, IOutput output)
        {
            _appDomain = appDomain;
            _assembly = assembly;
            _fs = fs;
            _output = output;
        }

        public AppDomain LoadEnvironmentFrom(string[] args)
        {
            var verbose = args.Any(x => x.ToLower().Contains("verbose"));
            var loadThese = new List<string>();

            var firstDllOrExeArgument = args.FirstOrDefault(x => x.EndsWith(".dll") || x.EndsWith(".exe"));
            if (firstDllOrExeArgument != null)
            {
                loadThese.Add(firstDllOrExeArgument);
            }

            loadThese.RemoveAll(x => x == null);

            if (!loadThese.Any())
            {
                var currentDirFiles = _fs.DirectoryGetFiles();
                loadThese.AddRange(currentDirFiles.Where(x => x.EndsWith(".dll")));
            }

            loadThese.ForEach(a => TryLoadIntoAppDomain(a, verbose));
            TryFindAndLoadConfig();

            return AppDomain.CurrentDomain;
        }

        private void TryLoadIntoAppDomain(string assemblyFile, bool verbose)
        {
            try
            {
                var assemblyName = _assembly.AssemblyNameGetAssemblyName(assemblyFile);
                _appDomain.CurrentDomainLoad(assemblyName);

                LoadedAssemblies.Add(assemblyName);
                _loadedPaths.Add(assemblyFile);

                if (verbose)
                {
                    _output.WriteLine("Loaded " + assemblyFile);
                }
            }
            catch (Exception ex)
            {
                if (verbose)
                {
                    _output.WriteLine($"Skipped loading '{assemblyFile}' because '{ex.Message}'.");
                }
            }
        }

        private void TryFindAndLoadConfig()
        {
            var searchPaths = new List<string>(_loadedPaths.Select(x=>x.Replace(".dll", ".dll.config")));
            searchPaths.AddRange(new[] { "web.config", "app.config" });
            var configsThatExist = searchPaths.Where(_fs.Exists).ToList();

            foreach (var config in configsThatExist)
            {
                if (TrySetConfiguration(config))
                {
                    return;
                }
            }
        }

        private bool TrySetConfiguration(string config)
        {
            try
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", config);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}