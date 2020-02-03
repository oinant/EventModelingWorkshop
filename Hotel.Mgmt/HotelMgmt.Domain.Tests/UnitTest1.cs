using System;
using System.Collections.Generic;
using System.Linq;
using HotelMgmt.Domain.CleaningRequest;
using NFluent;
using Xunit;

namespace HotelMgmt.Domain.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // given
            var pastStream = new List<IEvent>();
            var room = new Room(pastStream);
            var repo = new RepositoryStub<Room>(room);
            // when 
            var requestedAt = DateTime.Today.AddDays(5);
            var commandToApply = new CleaningRequestCommand()
            {
                RequestedAt = requestedAt,
                RoomId = 12
            };
            var commandHandler = new CleaningRequestHandler(repo);
            commandHandler.Handle(commandToApply);
            
            //then
            var expectedEvents = new List<IEvent>() {new CleaningRequested(12, "the boss", requestedAt)};
            Check.That(repo.ProducedEvents).ContainsExactly(expectedEvents);
        }
    }

    public class RepositoryStub<T> : IRepo<T> where T : class, IAggregate
    {
        private readonly T _aggregate;

        public IList<IEvent> ProducedEvents { get; private set; }

        public RepositoryStub(T aggregate)
        {
            _aggregate = aggregate;
        }
        
        public T GetById(int id)
        {
            return _aggregate;
        }

        public void Save(T aggregate)
        {
            ProducedEvents = aggregate.Produced.Select(x=> x).ToList();
            aggregate.CommitEvents();
        }
    }  


    //public abstract class Specification<TCommandHandler, TAggregate> where TCommandHandler : IHandleCommand<ICommand>
    //                                                                 where TAggregate : IAggregate
    //{
    //    protected abstract TAg Given();
    //    protected abstract void When();

    //    protected void Then(IList<IEvent> events)
    //    {
    //        var aggregate = 
    //    }
    //}
}
