using System;
using HotelMgmt.Domain;
using HotelMgmt.Domain.Abstractions;
using HotelMgmt.Infrastructure.Core.EventStore;

namespace HotelMgmt.Infrastructure
{

    public class RoomRepository : IRepo<RoomCleaning>
    {
        private readonly IStoreEvents _eventStore;

        public RoomRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public RoomCleaning GetById(Guid id)
        {
            var eventStream = _eventStore.GetStreamForAggregate(id);
            var room = new RoomCleaning(eventStream);
            return room;
        }


        public void Save(RoomCleaning roomCleaning)
        {
            _eventStore.AddEventsToStream(roomCleaning.Id, roomCleaning.Produced);
            roomCleaning.CommitEvents();

        }
    }
}