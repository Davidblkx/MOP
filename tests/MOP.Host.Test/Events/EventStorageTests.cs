using MOP.Infra.Domain.Events;
using MOP.Host.Events;
using MOP.Host.Test.Mocks;
using Optional.Unsafe;
using Xunit;

namespace MOP.Host.Test.Events
{
    [Collection("HOST")]
    public class EventStorageTests
    {
        public IEventStorage Storage { get; }

        public EventStorageTests(MockBuilder mock)
        {
            Storage = mock.Storage;
        }

        [Fact]
        public void TestWriteReadEvents()
        {
            var target1 = new Event<Unit>("UNIT", new Unit());
            var target2 = new Event<string>("STRIGN", "batatas");
            var target3 = new Event<People>("PEOPLE", new People("d1", 18));

            TestWriteTypeEvent(target1);
            TestWriteTypeEvent(target2);
            TestWriteTypeEvent(target3);
        }

        private void TestWriteTypeEvent<T>(IEvent<T> @event)
        {
            Storage.WriteEvent(@event);
            var optionRes = Storage.GetEvent(@event.Id);
            var value = optionRes.ValueOrDefault();

            Assert.True(optionRes.HasValue);
            Assert.NotNull(value);

            Assert.Equal(@event.Id, value.Id);

            var castValue = value.Cast<T>();
            Assert.NotNull(castValue);
            Assert.Equal(value.Body.HasValue, castValue.Body.HasValue);

            var bodyValue = castValue.Body.ValueOrDefault();
            Assert.NotNull(bodyValue);
            Assert.IsType<T>(bodyValue);
        }

        private class People
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public People(string n, int a)
            {
                Name = n;
                Age = a;
            }
        }
    }
}
