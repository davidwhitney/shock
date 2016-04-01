using NUnit.Framework;
using Shock.ArgumentParsing;

namespace Shock.Test.Unit.ArgumentParsing
{
    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void Continue_WhenKeyInDictionary_IsTrue()
        {
            var args = Arguments.With("continue");

            Assert.That(args.Continue, Is.True);
        }

        [TestCase("Help")]
        [TestCase("?")]
        public void Help_WhenKeyInDictionary_IsTrue(string helpArg)
        {
            var args = Arguments.With(helpArg);

            Assert.That(args.Help, Is.True);
        }

        [Test]
        public void Interactive_WhenKeyInDictionary_IsTrue()
        {
            var args = Arguments.With("interactive");

            Assert.That(args.Interactive, Is.True);
        }
    }
}
