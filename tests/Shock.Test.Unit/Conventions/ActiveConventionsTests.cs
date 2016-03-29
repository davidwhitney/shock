using NUnit.Framework;
using Shock.Conventions;

namespace Shock.Test.Unit.Conventions
{
    [TestFixture]
    public class ActiveConventionsTests
    {
        [Test]
        public void Default_ReturnsStandardConventions()
        {
            Assert.DoesNotThrow(() => ActiveConventions.Default());
        }
    }
}
