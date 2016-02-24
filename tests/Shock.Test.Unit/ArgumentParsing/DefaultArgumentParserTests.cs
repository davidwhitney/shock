using System.Linq;
using NUnit.Framework;
using Shock.ArgumentParsing;

namespace Shock.Test.Unit.ArgumentParsing
{
    [TestFixture]
    public class DefaultArgumentParserTests
    {
        [Test]
        public void Ctor_WithArg_RawValueIsSet()
        {
            var raw = new[] { "raw" };

            var args = new DefaultArgumentParser().Parse(raw);

            Assert.That(args.Raw, Is.EqualTo(raw));
        }
        
        [Test]
        public void Ctor_SingleValue_MappedToDictionary()
        {
            var raw = new[] { "DoSomething" };

            var args = new DefaultArgumentParser().Parse(raw);

            Assert.That(args.First().Key, Is.EqualTo("DoSomething"));
        }
    }
}
