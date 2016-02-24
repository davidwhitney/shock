using System;

namespace Shock.Test.Unit.FakesAndStubs
{
    /// <summary>
    /// Detected naming convention "Ends with 'Tasks'"
    /// </summary>
    public class FakeTaskClassTasks
    {
        public void DoSomething()
        {
            Console.WriteLine("Hi");
        }

        public void DoSomethingWithAParameter(string abc)
        {
        }

        public string DoSomethingWithAReturnValue()
        {
            return "something";
        }
    }
}