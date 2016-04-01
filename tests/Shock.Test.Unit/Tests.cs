using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Shock.Logging;

namespace Shock.Test.Unit
{
    public abstract class Tests<T>
    {
        public Fixture Fixture { get; set; }
        public T Sut { get; set; }
        protected ListOutput Output;

        [SetUp]
        protected void SetUpAutoTests()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());

            Output = new ListOutput();
            AddStub<IOutput>(Output);

            typeof (T).GetConstructors().ToList()
                .ForEach(ctor => ctor.GetParameters().ToList()
                    .ForEach(param =>
                    {
                        Fixture.Freeze(param.ParameterType);
                        Fixture.Freeze(typeof (Mock<>).MakeGenericType(param.ParameterType));
                    }));

            Setup();
            Sut = Fixture.Create<T>();
        }

        protected virtual void Setup() { }

        protected Mock<TMockType> Mock<TMockType>() where TMockType : class
        {
            return Fixture.Freeze<Mock<TMockType>>();
        }

        protected void AddStub<TMockType>(TMockType instance) where TMockType : class
        {
            Fixture.Register(() => instance);
        }

        protected void AddStub<TMockType>(Func<TMockType> instanceFunc) where TMockType : class
        {
            Fixture.Register(instanceFunc);
        }
    }
}