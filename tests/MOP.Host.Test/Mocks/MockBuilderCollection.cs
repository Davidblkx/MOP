using Xunit;

namespace MOP.Host.Test.Mocks
{
    [CollectionDefinition("HOST")]
    public class MockBuilderCollection : ICollectionFixture<MockBuilder>
    {
    }
}
