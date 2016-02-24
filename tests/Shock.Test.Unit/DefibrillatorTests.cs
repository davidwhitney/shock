using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Shock.ArgumentParsing;
using Shock.Execution;
using Shock.TaskDiscovery;
using Shock.Test.Unit.FakesAndStubs;

namespace Shock.Test.Unit
{
    [TestFixture]
    public class DefibrillatorTests : Tests<Defibrillator>
    {
        private List<MethodInfo> _tasks;
        private Arguments _args;

        protected override void Setup()
        {
            var methodInfo = typeof(FakeTaskClass).GetMethod("DoSomething");
            _tasks = new List<MethodInfo> { methodInfo };
            _args = new Arguments {{"DoSomething", new Arguments.Nothing()}};

            Mock<IExecuteATask>();
            Mock<IDiscoverTasks>().Setup(x => x.FindTasks(_args)).Returns(_tasks);
            Mock<ISelectTasksToRun>().Setup(x => x.SelectTasksFrom(_tasks, _args)).Returns(_tasks);
        }

        [Test]
        public void Shock_CallsExecutorWithSelectedTaskMethod()
        {
            Sut.Shock(_args);

            Mock<IExecuteATask>().Verify(x=>x.TryExecuteTask(_tasks[0], _args), Times.Once);
        }
    }
}
