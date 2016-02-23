namespace Shock.Test.Unit.FakesAndStubs
{
    public class FakeTaskClass
    {
        public static bool WasCalled { get; set; }

        public void DoSomething()
        {
            WasCalled = true;
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