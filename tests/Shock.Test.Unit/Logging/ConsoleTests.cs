using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Shock.Test.Unit.Logging
{
    public class ConsoleTests : Tests<Shock.Logging.Console>
    {
        [Test]
        public void WriteLine_WritesToConsole()
        {
            var sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                Console.SetOut(tw);

                Sut.WriteLine("Hi");
            }

            Assert.That(sb.ToString(), Does.StartWith("Hi"));
        }
    }
}
