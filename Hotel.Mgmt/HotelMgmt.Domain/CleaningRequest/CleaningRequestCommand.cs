using System;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Domain.CleaningRequest
{
    public class CleaningRequestCommand : ICommand
    {
        public Guid RoomId { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}