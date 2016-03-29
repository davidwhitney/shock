using NUnit.Framework;
using Shock.Activation;

namespace Shock.Test.Unit.Activation
{
    [TestFixture]
    public class ActivatorDependencyResolverTests : Tests<ActivatorDependencyResolver>
    {
        [Test]
        public void CreateT_NoConstructorParams_ReturnsT()
        {
            var instance = Sut.Create<NoCtorParams>();

            Assert.That(instance, Is.Not.Null);
        }

        public class NoCtorParams { }
    }
}
