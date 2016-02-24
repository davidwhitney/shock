using System;

namespace Shock.Test.Unit.FakesAndStubs
{
    public class FakeTaskClass2
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