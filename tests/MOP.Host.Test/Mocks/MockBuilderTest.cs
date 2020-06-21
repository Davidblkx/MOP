using Xunit;

namespace MOP.Host.Test.Mocks
{
    public class MockBuilderTest
    {
        [Fact]
        public void TestBuildMockHost()
        {
            var host = MockBuilder.BuildHost();
            Assert.NotNull(host);
            Assert.NotNull(host.LogService);
        }
    }
}
