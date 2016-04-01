using System;
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
            var methodInfo = typeof (FakeTaskClass).GetMethod("DoSomething");
            _tasks = new List<MethodInfo> { methodInfo };
            _args = new Arguments {{"DoSomething", new Arguments.Nothing()}};

            Mock<IExecuteATask>().Setup(x => x.TryExecuteTask(It.IsAny<MethodInfo>(), _args)).Returns(new TaskStatus(methodInfo));
            Mock<IDiscoverTasks>().Setup(x => x.FindTasks(_args, It.IsAny<List<Type>>())).Returns(_tasks);
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

            var exitCode = Sut.Shock(_args);

            Assert.That(exitCode, Is.EqualTo(ExitCodes.NoTasksRun));
            Assert.That(Sut.DiscoveredTasks.Count, Is.EqualTo(0));
            Assert.That(Output.Buffer, Does.Contain("usages:"));
            Assert.That(Output.Buffer.Count, Is.AtLeast(4));
        }

        [Test]
        public void Shock_NoArgsButOnlyOneTask_RunsTask()
        {
            _args.Raw = new[] { "" };

            var exitCode = Sut.Shock(_args);

            Assert.That(exitCode, Is.EqualTo(ExitCodes.Success));
            Assert.That(Sut.DiscoveredTasks.Count, Is.EqualTo(1));
            Assert.That(Sut.SelectedTasks.Count, Is.EqualTo(1));
            Assert.That(Sut.Results.Count, Is.EqualTo(1));
            Assert.That(Output.Buffer, Does.Contain("Executed: Shock.Test.Unit.FakesAndStubs.FakeTaskClass.DoSomething"));
        }

        [TestCase("?")]
        [TestCase("help")]
        public void Shock_QuestionMarkOrHelp_DoesntExecuteReturnsHelp(string arg)
        {
            _tasks.Add(typeof(FakeTaskClass).GetMethod("DoSomethingWithAReturnValue"));
            _args.Clear();
            _args.Add(arg, "");

            var exitCode = Sut.Shock(_args);

            Assert.That(exitCode, Is.EqualTo(ExitCodes.NoTasksRun));
            Assert.That(Sut.Results, Is.Empty);
            Assert.That(Output.Buffer, Does.Contain("usages:"));
            Assert.That(Output.Buffer, Does.Contain("tasks:" + Environment.NewLine));
            _tasks.ForEach(x => Assert.That(Sut.DiscoveredTasks, Does.Contain(x)));
        }

        [Test]
        public void Shock_TaskThrowsAnErrorButContinueArgPresent_ContinuesProcessing()
        {
            Sut.Executor = new DefaultTaskExecutor();
            _tasks.Insert(0, typeof(FakeTaskClass).GetMethod("Throw"));
            _args.Add("continue", "");

            var exitCode = Sut.Shock(_args);

            Assert.That(exitCode, Is.EqualTo(ExitCodes.Success));
            Assert.That(Sut.Results.Count, Is.EqualTo(2));
        }

        [Test]
        public void Shock_TaskThrowsAnError_OnlyProcessesUpToFailingTask()
        {
            Sut.Executor = new DefaultTaskExecutor();
            _tasks.Insert(0, typeof(FakeTaskClass).GetMethod("Throw"));

            var exitCode = Sut.Shock(_args);

            Assert.That(exitCode, Is.EqualTo(ExitCodes.Failed));
            Assert.That(Sut.Results.Count, Is.EqualTo(1));
        }

        [Test]
        public void Shock_TaskThrowsAnError_WritesExceptionDetails()
        {
            Sut.Executor = new DefaultTaskExecutor();
            _tasks.Clear();
            _tasks.Insert(0, typeof(FakeTaskClass).GetMethod("Throw"));

            Sut.Shock(_args);

            Assert.That(Output.Buffer[1], Does.Contain("System.NotImplementedException: Nope!"));
        }
    }
}
