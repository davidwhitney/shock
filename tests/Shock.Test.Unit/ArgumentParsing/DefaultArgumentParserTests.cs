using System.Linq;
using NUnit.Framework;
using Shock.ArgumentParsing;

namespace Shock.Test.Unit.ArgumentParsing
{
    [TestFixture]
    public class DefaultArgumentParserTests
    {
        [Test]
        public void Parse_WithArg_RawValueIsSet()
        {
            var raw = new[] { "raw" };

            var args = new DefaultArgumentParser().Parse(raw);

            Assert.That(args.Raw, Is.EqualTo(raw));
        }
        
        [TestCase("DoSomething")]
        [TestCase("/DoSomething")]
        [TestCase("--DoSomething")]
        [TestCase("-DoSomething")]
        public void Parse_SingleValue_MappedToDictionary(string param)
        {
            var raw = new[] { param };

            var args = new DefaultArgumentParser().Parse(raw);

            Assert.That(args.First().Key, Is.EqualTo("DoSomething"));
        }

        [Test]
        public void Parse_PairOfValues_MappedToDictionary()
        {
            var raw = new[] { "DoSomething=now" };

            var args = new DefaultArgumentParser().Parse(raw);

            Assert.That(args.First().Key, Is.EqualTo("DoSomething"));
            Assert.That(args.First().Value, Is.EqualTo("now"));
        }
    }
}
