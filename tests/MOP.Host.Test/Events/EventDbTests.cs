using MOP.Core.Domain.Events;
using MOP.Host.Events;
using Optional.Unsafe;
using Xunit;

namespace MOP.Host.Test.Events
{
    public class EventDbTests
    {
        [Fact]
        public void TestEventDbToIEvent()
        {
            var subject = new Event<Unit>("EVENT", new Unit());
            
            var targetEventDb = EventDb.From(subject);
            var targetEvent = targetEventDb.ToEvent();
            var targetCast = targetEvent.Cast<Unit>();

            Assert.True(subject.Body.HasValue);
            Assert.NotNull(targetEventDb);
            Assert.NotNull(targetEventDb.Body);
            Assert.True(targetEvent.Body.HasValue);
            Assert.True(targetCast.Body.HasValue);
            Assert.IsType<Unit>(targetCast.Body.ValueOrFailure());
        }
    }
}
