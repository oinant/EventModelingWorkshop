using System;
using System.Collections.Generic;
using HotelMgmt.Domain.Abstractions;
using HotelMgmt.Domain.CleaningRequest;
using HotelMgmt.Domain.Rooms;
using NFluent;
using Xunit;

namespace HotelMgmt.Domain.Tests
{
    public class UnitTest1
    {
        private RoomReferential rooms;

        public UnitTest1()
        {
            rooms = new HotelMgmt.Domain.Rooms.RoomReferential();
        }

        [Fact]
        public void Test1()
        {
            // given
            var pastStream = new EventStream(rooms.GetByNumber(12).Id);
            var roomCleaning = new RoomCleaning(pastStream);
            var repo = new RepositoryStub<RoomCleaning>(roomCleaning);
            // when 
            var requestedAt = DateTime.Today.AddDays(5);
            var commandToApply = new CleaningRequestCommand()
            {
                RequestedAt = requestedAt,
                RoomId = rooms.GetByNumber(12).Id
            };
            var commandHandler = new CleaningRequestHandler(repo);
            commandHandler.Handle(commandToApply);
            
            //then
            var expectedEvents = new List<IEvent>() {new CleaningRequested(rooms.GetByNumber(12).Id, "the boss", requestedAt)};
            Check.That(repo.ProducedEvents).ContainsExactly(expectedEvents);
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
