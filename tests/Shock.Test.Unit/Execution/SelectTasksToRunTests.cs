using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Shock.ArgumentParsing;
using Shock.Execution;
using Shock.Test.Unit.FakesAndStubs;

namespace Shock.Test.Unit.Execution
{
    [TestFixture]
    public class SelectTasksToRunTests : Tests<SelectTasksToRun>
    {
        private List<MethodInfo> _tasksFromDomain;

        protected override void Setup()
        {
            _tasksFromDomain = new List<MethodInfo> { typeof(FakeTaskClass).GetMethod("DoSomething") };
        }

        [Test]
        public void SelectTasksToRun_MethodNameMatchesAvailableMethodExactly_Selects()
        {
            var tasksToRun = Sut.SelectTasksFrom(_tasksFromDomain, Arguments.With("DoSomething"));

            Assert.That(tasksToRun[0], Is.EqualTo(_tasksFromDomain[0]));
        }

        [Test]
        public void SelectTasksToRun_MethodNameProvidedAndDoesntMatch_ReturnsEmpty()
        {
            var tasksToRun = Sut.SelectTasksFrom(_tasksFromDomain, Arguments.With("NoMatchingMethodName"));

            Assert.That(tasksToRun, Is.Empty);
        }
    }
}
