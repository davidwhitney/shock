using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shock.ArgumentParsing;
using Shock.TaskDiscovery;
using Shock.Test.Unit.FakesAndStubs;

namespace Shock.Test.Unit.TaskDiscovery
{
    [TestFixture]
    public class DefaultTaskDiscovererTests
    {
        private DefaultTaskDiscoverer _disc;
        private List<Type> _candidates;

        [SetUp]
        public void SetUp()
        {
            _disc = new DefaultTaskDiscoverer();
            _candidates = new List<Type>();
        }

        [Test]
        public void FindTasks_GivenEmptyCandidatesCollection_ReturnsNothing()
        {
            var discovered = _disc.FindTasks(new Arguments(), _candidates);

            Assert.That(discovered, Is.Empty);
        }

        [Test]
        public void FindTasks_CandidatesContainsMethodWithTaskAttribute_FindsMethod()
        {
            _candidates.Add(typeof(FakeTaskClassDueToAttributes));

            var discovered = _disc.FindTasks(new Arguments(), _candidates);

            Assert.That(discovered.Count, Is.EqualTo(1));
            Assert.That(discovered[0].Name, Is.EqualTo("DoSomething"));
        }

        [Test]
        public void FindTasks_TypeNameContainsTask_FindsMethods()
        {
            _candidates.Add(typeof(TypeNameContainsTaskSoAllTheseMethodsAreSelected));

            var discovered = _disc.FindTasks(new Arguments(), _candidates);

            Assert.That(discovered.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void FindTasks_TypeNameEndsWithTasks_FindsMethods()
        {
            _candidates.Add(typeof(AllTheseAreValidBecauseTypeEndsInTasks));

            var discovered = _disc.FindTasks(new Arguments(), _candidates);

            Assert.That(discovered.Count, Is.EqualTo(2));
        }

        [TestCase(typeof(System.ShockTests.ExcludedDuetoNamespaceTask))]
        [TestCase(typeof(Accessibility.ShockTests.ExcludedDuetoNamespaceTask))]
        [TestCase(typeof(Microsoft.ShockTests.ExcludedDuetoNamespaceTask))]
        public void FindTasks_NamespaceIsExcludedDueToStartString_ExcludedFromTaskList(Type t)
        {
            _candidates.Add(t);

            var discovered = _disc.FindTasks(new Arguments(), _candidates);

            Assert.That(discovered, Is.Empty);
        }
    }

}

namespace System.ShockTests { public class ExcludedDuetoNamespaceTask { public void A() { } } }
namespace Accessibility.ShockTests { public class ExcludedDuetoNamespaceTask { public void A() { } } }
namespace Microsoft.ShockTests { public class ExcludedDuetoNamespaceTask { public void A() { } } }
