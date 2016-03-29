using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Shock.ArgumentParsing;
using Shock.Execution;
using Shock.Logging;
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
            var methodInfo = typeof (FakeTaskClass).GetMethod("DoSomething");
            _tasks = new List<MethodInfo> { methodInfo };
            _args = new Arguments {{"DoSomething", new Arguments.Nothing()}};

            Mock<IExecuteATask>().Setup(x => x.TryExecuteTask(It.IsAny<MethodInfo>(), _args)).Returns(new TaskStatus(methodInfo));
            Mock<IDiscoverTasks>().Setup(x => x.FindTasks(_args)).Returns(_tasks);
            Mock<ISelectTasksToRun>().Setup(x => x.SelectTasksFrom(_tasks, _args)).Returns(_tasks);
        }

        [Test]
        public void Shock_CallsExecutorWithSelectedTaskMethod()
        {
            Sut.Shock(_args);

            Mock<IExecuteATask>().Verify(x=>x.TryExecuteTask(_tasks[0], _args), Times.Once);
        }

        [Test]
        public void Shock_NoTasksDiscovered_OutputsHelpAboutUsage()
        {
            _tasks.Clear();

            Sut.Shock(_args);

            Assert.That(Output.Buffer, Does.Contain("usages:"));
            Assert.That(Output.Buffer.Count, Is.EqualTo(4));
        }

        [Test]
        public void Shock_NoArgsButOnlyOneTask_RunsTask()
        {
            _args.Raw = new[] { "" };

            Sut.Shock(_args);

            Assert.That(Sut.Results.Count, Is.EqualTo(1));
            Assert.That(Output.Buffer, Does.Contain("Executed: Shock.Test.Unit.FakesAndStubs.FakeTaskClass.DoSomething"));
        }

        [Test]
        public void Shock_NoArgsButMultipleTasks_DoesntExecuteReturnsHelp()
        {
            _tasks.Add(typeof(FakeTaskClass).GetMethod("DoSomethingWithAReturnValue"));
            _args.Raw = new[] { "" };

            Sut.Shock(_args);

            Assert.That(Sut.Results, Is.Empty);
            Assert.That(Output.Buffer, Does.Contain("usages:"));
            Assert.That(Output.Buffer, Does.Contain("tasks:" + Environment.NewLine));
            foreach (var task in _tasks)
            {
                Assert.That(Output.Buffer, Does.Contain($"\t{task.DeclaringType.Namespace}.{task.DeclaringType.Name}.{task.Name}{Environment.NewLine}"));
            }
        }

        [Test]
        public void Shock_DiscoversTasksButNoneAreSelectedDueToInappropriateCliArgs_PrintsTasks()
        {
            _args.Raw = new[] {""};

            Sut.Shock(_args);

            Assert.That(Output.Buffer, Is.Not.Empty);
        }
    }
}
