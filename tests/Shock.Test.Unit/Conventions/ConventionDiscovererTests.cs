using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Shock.Conventions;
using Shock.Execution;
using Shock.TaskDiscovery;
using SelectionFunc = System.Func<System.Collections.Generic.List<System.Reflection.MethodInfo>, System.Collections.Generic.Dictionary<string, object>, System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>>;

namespace Shock.Test.Unit.Conventions
{
    [TestFixture]
    public class ConventionDiscovererTests : Tests<ConventionDiscoverer>
    {
        private ActiveConventions _conventions;

        protected override void Setup()
        {
            _conventions = new ActiveConventions();
        }

        [Test]
        public void AdjustConventions_NoClassesAvailableToModifyConventionsFrom_DefaultsReturned()
        {
            Sut.AdjustConventions(_conventions, new List<Type>());
        }

        [Test]
        public void AjustConventions_ClassThatModifiesConventionsDiscovered_InvokesInstance()
        {
            var classes = new List<Type> {typeof (ThisClassModifiesShockConventions)};

            Sut.AdjustConventions(_conventions, classes);

            Assert.That(ThisClassModifiesShockConventions.Called, Is.True);
        }

        [Test]
        public void AdjustConventions_ClassThatModifiesDiscoveryRules_RulesAreModified()
        {
            var td = new DefaultTaskDiscoverer();
            _conventions.TaskDiscoverer = td;
            var classes = new List<Type> {typeof (ClassThatRemovesDefaultConventions)};

            Sut.AdjustConventions(_conventions, classes);

            Assert.That(td.Matches, Is.Empty);
            Assert.That(td.ExcludeTypes, Is.Empty);
        }

        [Test]
        public void AdjustConventions_ClassThatModifiesSelectionRules_RulesAreModified()
        {
            var td = new SelectTasksToRun();
            _conventions.TaskSelector = td;
            var classes = new List<Type> {typeof (ClassThatRemovesDefaultSelectionRules)};

            Sut.AdjustConventions(_conventions, classes);

            Assert.That(td.Matches, Is.Empty);
        }
    }

    public class ThisClassModifiesShockConventions
    {
        public static bool Called { get; set; }

        public ThisClassModifiesShockConventions(ActiveConventions conventions)
        {
            Called = true;
        }
    }

    public class ClassThatRemovesDefaultConventions
    {
        public void ConfigureTaskDiscovery(List<Func<MethodInfo, bool>> matchRules, List<Func<Type, bool>> exclusionRules)
        {
            matchRules.Clear();
            exclusionRules.Clear();
        }
    }

    public class ClassThatRemovesDefaultSelectionRules
    {
        public void ConfigureTaskSelection(List<SelectionFunc> matchRules)
        {
            matchRules.Clear();
        }
    }
}
