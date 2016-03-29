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
        protected override void Setup()
        {
            Mock<IAssemblyWrapper>()
                .Setup(x => x.AssemblyNameGetAssemblyName(It.IsAny<string>()))
                .Returns((string name) => new AssemblyName(name));
        }

        [Test]
        public void LoadEnvironmentFrom_LoadsEachDllInTheCliArgs()
        {
            var args = new List<string> { "abc.dll" };

            Sut.LoadEnvironmentFrom(args.ToArray());

            Assert.That(Sut.LoadedAssemblies[0].Name, Is.EqualTo("abc.dll"));
        }
    }
}
