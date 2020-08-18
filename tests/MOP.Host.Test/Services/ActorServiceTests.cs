using Akka.Actor;
using MOP.Infra.Domain.Actors;
using MOP.Host.Services;
using MOP.Host.Test.Mocks;
using Optional;
using Xunit;
using Moq;
using Optional.Unsafe;
using System;
using System.Linq;


using static MOP.Infra.Domain.Actors.IActorRefInstanceType;
using System.Collections.Generic;
using MOP.Infra.Domain.Host;
using MOP.Infra.Services;

namespace MOP.Host.Test.Services
{
    [Collection("HOST")]
    public class ActorServiceTests
    {
        private IHost Host { get; }
        private MockBuilder Builder { get; }

        public ActorServiceTests(MockBuilder mock)
        {
            Host = mock.Host;
            Builder = mock;
        }

        private IActorService CreateService()
        {
            return Builder.BuildActorService();
        }

        [Fact]
        public void AddActorFactory_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory1 = new TestActorFactory("test1", Singleton);
            IActorFactory factory2 = new TestActorFactory("test1", Transient);

            // Act
            var result1 = service.AddActorFactory(factory1);
            var result2 = service.AddActorFactory(factory2, false);
            var result3 = service.AddActorFactory(factory2);

            // Assert
            Assert.True(result1);
            Assert.False(result2);
            Assert.True(result3);
        }

        [Fact]
        public void GetActorFactory_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory1 = new TestActorFactory("test1", Singleton);
            IActorFactory factory2 = new TestActorFactory("test1", Transient);

            // Act
            var getRes1 = service.GetActorFactory(factory1.ActorRefName);
            var addRes1 = service.AddActorFactory(factory1);
            var getRes2 = service.GetActorFactory(factory1.ActorRefName);
            var addRes2 = service.AddActorFactory(factory2, true);
            var getRes3 = service.GetActorFactory(factory1.ActorRefName);

            // Assert
            Assert.False(getRes1.HasValue);

            Assert.True(addRes1);
            Assert.True(getRes2.HasValue);
            var val1 = getRes2.ValueOrFailure();
            Assert.Equal(factory1.ActorRefName, val1.ActorRefName);
            Assert.Equal(factory1.InstanceType, val1.InstanceType);

            Assert.True(addRes2);
            Assert.True(getRes3.HasValue);
            var val2 = getRes3.ValueOrFailure();
            Assert.Equal(factory2.ActorRefName, val2.ActorRefName);
            Assert.Equal(factory2.InstanceType, val2.InstanceType);
        }

        [Fact]
        public void GetActorOf_StateUnderTest_ExpectedBehavior_Transient()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory = new TestActorFactory("test", Transient);

            // Act
            service.AddActorFactory(factory);
            var actor1 = service.GetActorOf(factory.ActorRefName).ValueOrFailure();
            var actor2 = service.GetActorOf(factory.ActorRefName).ValueOrFailure();

            var name1 = actor1.Path.Name;
            var name2 = actor2.Path.Name;

            // Assert
            Assert.NotNull(name1);
            Assert.NotNull(name2);
            Assert.NotEqual(name1, name2);
        }

        [Fact]
        public void GetActorOf_StateUnderTest_ExpectedBehavior_Singleton()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory = new TestActorFactory("test", Singleton);

            // Act
            service.AddActorFactory(factory);
            var actor1 = service.GetActorOf(factory.ActorRefName).ValueOrFailure();
            var actor2 = service.GetActorOf(factory.ActorRefName).ValueOrFailure();

            var name1 = actor1.Path.Name;
            var name2 = actor2.Path.Name;

            // Assert
            Assert.NotNull(name1);
            Assert.NotNull(name2);
            Assert.Equal(name1, name2);
        }

        [Fact]
        public void GetActorRefFactories_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory1 = new TestActorFactory("test1", Singleton);
            IActorFactory factory2 = new TestActorFactory("test2", Transient);

            // Act
            service.AddActorFactory(factory1);
            service.AddActorFactory(factory2);
            var list = service.GetActorRefFactories();

            // Assert
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public void HasActorFactory_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            IActorFactory factory1 = new TestActorFactory("test1", Singleton);

            // Act
            var hasRes1 = service.HasActorFactory(factory1.ActorRefName);
            var addRes1 = service.AddActorFactory(factory1);
            var hasRes2 = service.HasActorFactory(factory1.ActorRefName);
            var hasRes3 = service.HasActorFactory("batatas");

            // Assert
            Assert.False(hasRes1);
            Assert.True(addRes1);
            Assert.True(hasRes2);
            Assert.False(hasRes3);
        }

        private class TestActorFactory : IActorFactory
        {
            public string ActorRefName { get; }
            public IActorRefInstanceType InstanceType { get; }

            public TestActorFactory(string name, IActorRefInstanceType type)
            {
                ActorRefName = name;
                InstanceType = type;
            }

            public Option<IActorRef> BuildActorRef(ActorSystem actorSystem)
            {
                var mock = new Mock<IActorRef>();

                var guid = Guid.NewGuid().ToString();
                mock.Setup(e => e.Path).Returns(new TestActorPath(guid));
                return Option.Some(mock.Object);
            }
        }

        private class TestActorPath : ActorPath
        {
            public TestActorPath(string name) 
                : base(Address.Parse("akka://MySystem"), name){ }

            public override IReadOnlyList<string> Elements => throw new NotImplementedException();
            public override ActorPath Root => throw new NotImplementedException();
            public override ActorPath Parent => throw new NotImplementedException();
            public override int CompareTo(ActorPath other)
            {
                throw new NotImplementedException();
            }

            public override ActorPath WithUid(long uid)
            {
                throw new NotImplementedException();
            }
        }
    }
}
