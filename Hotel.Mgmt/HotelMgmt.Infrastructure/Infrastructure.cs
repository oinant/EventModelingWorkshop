using System;
using System.Collections.Generic;
using HotelMgmt.Domain;

namespace HotelMgmt.Infrastructure
{

    public class RoomRepository : IRepo<Room>
    {
        private readonly IStoreEvents _eventStore;

        public RoomRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public Room GetById(int id)
        {
            var eventStream = _eventStore.GetStreamForAggregate(id);
            var room = new Room(eventStream);
            return room;
        }


        public void Save(Room room)
        {
            _eventStore.AddEventsToStream(room.Id, room.Produced);
            room.CommitEvents();

        }
    }
}