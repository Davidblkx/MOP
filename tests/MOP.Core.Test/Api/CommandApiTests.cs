using MOP.Core.Domain.Api;
using System.Linq;
using Xunit;

namespace MOP.Core.Test.Api
{
    public class CommandApiTests
    {
        [Fact]
        public void TestCommandApi()
        {
            var t = ActorInstanceFactory
                .ForType<TestGroup>()
                .Build();

            Assert.NotNull(t);
            Assert.Equal(TestGroup.GRP_PATH, t.Path);
            Assert.Equal(TestGroup.GRP_DESC, t.Description);

            Assert.Single(t.Commands);
            var m = t.Commands.FirstOrDefault();

            Assert.NotNull(m);
            Assert.Equal(TestGroup.ACT_DESC, m.Description);
            Assert.Equal(3, m.Arguments.Count);
            Assert.Equal("DoAction", m.Name);
            Assert.Equal(typeof(string), m.ReturnType);

            var prop1 = m.Arguments[0];
            Assert.NotNull(prop1);
            Assert.Equal("prop1", prop1.Name);
            Assert.Equal(typeof(int), prop1.ArgumentType);
            Assert.Equal(0, prop1.Position);

            var prop2 = m.Arguments[1];
            Assert.NotNull(prop2);
            Assert.Equal("prop2", prop2.Name);
            Assert.Equal(typeof(long), prop2.ArgumentType);
            Assert.Equal(1, prop2.Position);

            var prop3 = m.Arguments[2];
            Assert.NotNull(prop3);
            Assert.Equal("prop3", prop3.Name);
            Assert.Equal(typeof(string), prop3.ArgumentType);
            Assert.Equal(2, prop3.Position);
            Assert.True(prop3.IsOptional);
            Assert.True(prop3.HasDefaultValue);
            Assert.Equal(TestGroup.DEFAULT_VALUE, prop3.DefaultValue);
        }
    }

    [ActorInstance(GRP_PATH, GRP_DESC)]
    internal class TestGroup
    {
        public const string GRP_PATH = "test/group";
        public const string GRP_DESC = "a test type description";
        public const string ACT_DESC = "do some action";
        public const string DEFAULT_VALUE = "batatas";

#pragma warning disable IDE0051 // Remove unused private members
        [ActorAction(ACT_DESC)]
        private string DoAction(int prop1, long prop2, string prop3 = DEFAULT_VALUE)
            => $"{prop1}{prop2}{prop3}";
#pragma warning restore IDE0051 // Remove unused private members
    }
}
