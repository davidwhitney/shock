using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Shock.AppDomainShims;
using Shock.EnvironmentDiscovery;

namespace Shock.Test.Unit.EnvironmentDiscovery
{
    [TestFixture]
    public class DetectAndLoadRelevantAssembliesTests : Tests<DetectAndLoadRelevantAssemblies>
    {
        private List<string> _currentDirectory;

        protected override void Setup()
        {
            Mock<IAssemblyWrapper>()
                .Setup(x => x.AssemblyNameGetAssemblyName(It.IsAny<string>()))
                .Returns((string name) => new AssemblyName(name));

            _currentDirectory = new List<string>();
            Mock<IFileSystemWrapper>()
                .Setup(x => x.DirectoryGetFiles(It.IsAny<string>()))
                .Returns(()=> _currentDirectory.ToArray());
        }

        [TestCase("abc.dll")]
        [TestCase("abc.exe")]
        public void LoadEnvironmentFrom_LoadsEachDllOrExeInTheCliArgs(string arg)
        {
            var args = new List<string> { arg };

            Sut.LoadEnvironmentFrom(args.ToArray());

            Assert.That(Sut.LoadedAssemblies[0].Name, Is.EqualTo(arg));
        }

        [Test]
        public void LoadEnvironmentFrom_NoDllSpecified_LoadsAllDllsInDirectory()
        {
            _currentDirectory.Add("discovered1.dll");
            _currentDirectory.Add("discovered2.dll");

            Sut.LoadEnvironmentFrom(new List<string>().ToArray());

            Assert.That(Sut.LoadedAssemblies[0].Name, Is.EqualTo("discovered1.dll"));
            Assert.That(Sut.LoadedAssemblies[1].Name, Is.EqualTo("discovered2.dll"));
        }

        [Test]
        public void LoadEnvironmentFrom_FailsToLoadAssembly_DoesntCrashAndLogs()
        {
            _currentDirectory.Add("discovered1.dll");
            Mock<IAssemblyWrapper>().Setup(x => x.AssemblyNameGetAssemblyName(It.IsAny<string>())).Throws<Exception>();

            Sut.LoadEnvironmentFrom(new List<string>().ToArray());

            Assert.That(Sut.LoadedAssemblies, Is.Empty);
            Assert.That(Output.Buffer, Does.Contain("Skipped loading 'discovered1.dll' because 'Exception of type 'System.Exception' was thrown.'."));
        }
    }
}
