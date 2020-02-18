using System;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Domain.CleaningRequest
{
    public class CleaningRequested : IEvent
    {
        public int RoomNumber { get; }
        public string Requester { get; }
        public DateTime RequestedAt { get; }

        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public CleaningRequested(Guid roomId, string requester, DateTime requestedAt)
        {
            Id = roomId;
            RequestedAt = requestedAt;
            Requester = requester;

            CreatedAt = DateTime.Now;
        }
    }
}