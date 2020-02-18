using System;
using HotelMgmt.Domain.CleaningRequest;
using HotelMgmt.Infrastructure.Core;

namespace HotelMgmt.Projections
{
    public class CleaningRequestReportMessageHandler : IHandleEvents<CleaningRequested>
    {
        public void HandleEvent(CleaningRequested @event)
        {
            Console.WriteLine($"I got event! : ");
            Console.WriteLine($"[room : {@event.RoomNumber}, requester : {@event.RoomNumber}]");
        }
    }
}
