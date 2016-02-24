namespace Shock.Test.Unit.FakesAndStubs
{
    /// <summary>
    /// Detected naming convention "Ends with 'Tasks'"
    /// </summary>
    public class FakeTaskClassDueToAttributes
    {
        [Task]
        public void DoSomething()
        {
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