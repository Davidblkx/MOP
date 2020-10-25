using MOP.Core.Domain.Api;
using System.Linq;
using Xunit;

namespace MOP.Core.Test.Api
{
    public class ApiCommandTests
    {
        [Fact]
        public void TestApiBuild()
        {
            var api = ApiHostFactory.ForType<TestApi>();
            Assert.NotNull(api);

            Assert.Equal(TestApi.PATH, api.Path);
            Assert.Equal(TestApi.DESCRIPTION, api.Description);
            Assert.Equal(TestApi.NAME, api.Name);

            var actions = api.Actions;
            Assert.Equal(2, actions.Count());

            var voidType = typeof(void).AssemblyQualifiedName;
            var argType = typeof(Arg1).AssemblyQualifiedName;

            var a1 = actions.FirstOrDefault(e => e.Name == "DoAction1");
            Assert.NotNull(a1);
            Assert.Equal(TestApi.DESC_ARG, a1.Description);
            Assert.Equal("No description", a1.ReturnDescription);
            Assert.Equal(voidType, a1.ReturnType);
            Assert.Equal(argType, a1.MessageType);

            var a2 = actions.FirstOrDefault(e => e.Name == "DoAction2");
            Assert.NotNull(a2);
            Assert.Equal("No description", a2.Description);
            Assert.Equal(TestApi.DESC_RETURN, a2.ReturnDescription);
            Assert.Equal(voidType, a2.MessageType);
            Assert.Equal(argType, a2.ReturnType);
        }
    }

    [ApiHost(PATH, Description = DESCRIPTION, Name = NAME)]
    internal class TestApi
    {
        public const string PATH = "Test/Path";
        public const string DESCRIPTION = "a description";
        public const string NAME = "FriendlyName";
        public const string DESC_ARG = "Argument description";
        public const string DESC_RETURN = "Return Description";

        [ApiAction(Description = DESC_ARG)]
        public void DoAction1(Arg1 _) { }

        [ApiAction(Returns = DESC_RETURN)]
#pragma warning disable IDE0051 // Remove unused private members
        private Arg1 DoAction2() { return new Arg1(); }
#pragma warning restore IDE0051 // Remove unused private members
    }

    internal class Arg1
    {
        public string Data { get; set; }
        public string Name { get; set; }
    }
}
