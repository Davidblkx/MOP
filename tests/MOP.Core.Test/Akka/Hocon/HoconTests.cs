using MOP.Core.Akka.Hocon;
using MOP.Core.Infra.Extensions;
using Akka.Configuration.Hocon;
using Xunit;
using System.Threading.Tasks;

namespace MOP.Core.Test.Akka.Hocon
{
    public class HoconTests
    {
        [Fact]
        public void TestHoconFileLoader()
        {
            var file = HoconFileLoader.Load();
            Assert.False(file.IsNullOrEmpty());
        }

        [Fact]
        public void TestHoconConfigFactory()
        {
            var subject = new HoconConfigFactory(new HoconConfig());
            var target = subject.Build();

            Assert.False(target.IsNullOrEmpty());
            Assert.DoesNotContain("#!", target);

            var hoconObj = Parser.Parse(target, _ => new HoconRoot(new HoconValue()));
            Assert.NotNull(hoconObj);
            Assert.False(hoconObj.Value.IsEmpty);
        }
    }
}
