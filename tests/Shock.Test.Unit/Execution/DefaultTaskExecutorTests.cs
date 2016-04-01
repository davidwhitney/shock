using NUnit.Framework;
using Shock.ArgumentParsing;
using Shock.Execution;

namespace Shock.Test.Unit.Execution
{
    [TestFixture]
    public class DefaultTaskExecutorTests : Tests<DefaultTaskExecutor>
    {
        private static string _argRequired;

        [Test]
        public void TryExecuteTask_MethodDoesNotThrow_ReturnsSuccess()
        {
            var method = GetType().GetMethod("DoesntThrow");
            var args = new Arguments();

            var result = Sut.TryExecuteTask(method, args);

            Assert.That(result.ExecutedSuccessfully, Is.True);
        }

        [Test]
        public void TryExecuteTask_MethodRequiresParam_Executes()
        {
            var method = GetType().GetMethod("RequiresAnArgument");
            var args = Arguments.With("arg", "hi");

            var result = Sut.TryExecuteTask(method, args);

            Assert.That(result.ExecutedSuccessfully, Is.True);
            Assert.That(_argRequired, Is.EqualTo("hi"));
        }

        [Test]
        public void TryExecuteTask_MethodRequiresParamsOfMultipleTypes_Executes()
        {
            var method = GetType().GetMethod("RequiresAnArgumentOfTypes");
            var args = Arguments.With("aString", "hi");
            args.Add("anInt", "1");

            var result = Sut.TryExecuteTask(method, args);

            Assert.That(result.ExecutedSuccessfully, Is.True);
        }

        [Test]
        public void TryExecuteTask_MethodThrows_ReturnsFailure()
        {
            var method = GetType().GetMethod("Throws");
            var args = new Arguments();

            var result = Sut.TryExecuteTask(method, args);

            Assert.That(result.ExecutedSuccessfully, Is.False);
            Assert.That(result.Exception, Is.TypeOf<AssertionException>());
        }

        public void DoesntThrow() { }
        public void Throws() { throw new AssertionException("aha"); }
        public void RequiresAnArgument(string arg) { _argRequired = arg; }
        public void RequiresAnArgumentOfTypes(string aString, int anInt) { }
    }
}
