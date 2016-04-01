using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Shock.Test.Unit
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Program_NoArgs_FindsAndRunsDefaultTask()
        {
            var sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                Console.SetOut(tw);
                Program.Main(new string[0]);
            }

            Assert.That(sb.ToString(), Does.Contain("Executed: Shock.Test.Unit.FakesAndStubs.DefaultTask.Run"));
        }
    }
}
