using NUnit.Framework;
using Shock.Conventions;

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
        public void AdjustConventions_NoConventionsDiscovered_DefaultsReturned()
        {
            Sut.AdjustConventions(_conventions);
        }
    }
}
