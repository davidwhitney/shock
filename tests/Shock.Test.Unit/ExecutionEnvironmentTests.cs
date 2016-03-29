using System.Collections.Generic;
using NUnit.Framework;

namespace Shock.Test.Unit
{
    [TestFixture]
    public class ExecutionEnvironmentTests : Tests<ExecutionEnvironment>
    {
        private List<string> _cliArgs;

        protected override void Setup()
        {
            _cliArgs = new List<string>();
            AddStub(() => _cliArgs.ToArray());
        }

        [Test]
        public void Constructed_DefaultsNotNull()
        {
            Assert.That(Sut.Conventions, Is.Not.Null);
            Assert.That(Sut.Defibrillator, Is.Not.Null);
        }

        [Test]
        public void Dispose_DoesAbsolutelyNothing_BecauseItsOnlyADisposableForSyntaticSugar()
        {
            Assert.DoesNotThrow(()=>Sut.Dispose());
        }
    }
}
