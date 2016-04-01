using System;

namespace Shock.Test.Unit.FakesAndStubs
{
    public class FakeTaskClass
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

        public string Throw()
        {
            throw new NotImplementedException("Nope!");
        }
    }
}