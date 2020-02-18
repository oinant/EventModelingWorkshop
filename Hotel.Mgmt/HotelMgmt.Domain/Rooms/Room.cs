using System;

namespace HotelMgmt.Domain.Rooms
{
    public struct Room
    {
        private readonly Guid _id;
        public Guid Id => _id;

        public Room(string id)
        {
            _id = Guid.Parse(id);
        }
    }
}