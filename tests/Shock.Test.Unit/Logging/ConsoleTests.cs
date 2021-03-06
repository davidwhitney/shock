﻿using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Shock.Test.Unit.Logging
{
    [TestFixture]
    public class ConsoleTests : Tests<Shock.Logging.ConsoleOutput>
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
