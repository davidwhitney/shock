using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Shock.Test.Unit
{
    public abstract class Tests<T>
    {
        public Fixture Fixture { get; set; }
        public T Sut { get; set; }

        [SetUp]
        protected void SetUpAutoTests()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());

            typeof (T).GetConstructors().ToList()
                .ForEach(ctor => ctor.GetParameters().ToList()
                    .ForEach(param =>
                    {
                        Fixture.Freeze(param.ParameterType);
                        var mockType = typeof (Mock<>).MakeGenericType(param.ParameterType);
                        Fixture.Freeze(mockType);
                    }));

            Setup();
            Sut = Fixture.Create<T>();
        }

        protected virtual void Setup() { }

        protected Mock<TMockType> Mock<TMockType>() where TMockType : class
        {
            return Fixture.Freeze<Mock<TMockType>>();
        }
    }
}