using MOP.Infra.UserSettings;
using Xunit;

namespace MOP.Infra.Test.UserSettings
{
    public class UserSettingsFactoryTests
    {
        [Fact]
        public void TestSetDefaultValue()
        {
            var targetValue = new TestClass { Name = "batatas", Age = 30 };
            var target = UserSettingsFactory
                .Create<TestClass>()
                .SetDefaultValue(targetValue);

            Assert.NotNull(target);
            Assert.Equal(targetValue, target.DefaultValue);
        }

        [Fact]
        public void TestUseDefaultValue()
        {
            var target = UserSettingsFactory
                .Create<TestClass>();

            Assert.NotNull(target);
            var def = target.DefaultValue;
            Assert.NotNull(def);
            Assert.Equal("Unknown", def.Name);
            Assert.Equal(10, def.Age);
        }
    }

    internal class TestClass
    {
        public string Name { get; set; } = "Unknown";
        public int Age { get; set; } = 10;
    }
}
