using System;
using HotelMgmt.Domain;
using HotelMgmt.Domain.CleaningRequest;
using HotelMgmt.Infrastructure;

namespace HotelMgmt.Projections
{
    public class CleaningRequestReportMessageHandler : IHandleEvents
    {
        public void HandleEvent(IEvent @event)
        {
            Console.WriteLine($"I got event! : ");
            var castedEvent = (CleaningRequested) @event;

            Console.WriteLine($"[room : {castedEvent.RoomNumber}, requester : {castedEvent.RoomNumber}]");
        }
    }
}
