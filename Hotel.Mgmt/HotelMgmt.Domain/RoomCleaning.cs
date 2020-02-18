using System;
using System.Collections.Generic;
using HotelMgmt.Domain.Abstractions;
using HotelMgmt.Domain.CleaningRequest;

namespace HotelMgmt.Domain
{

    public class RoomCleaning : Aggregate
    {
        public string RequestedBy { get; set; }
        //public int RoomNumber { get; set; }

        public RoomCleaning(EventStream events) : base(events) { }

        public void RequestCleaning(in DateTime requestedAt)
        {
            var @event = new CleaningRequested(Id, "the boss", requestedAt);
            Apply(@event);
        }

        protected override void Evolve(IEvent @event)
        {
            switch (@event)
            {
                case CleaningRequested cleaningRequested:
                    this.RequestedBy = cleaningRequested.Requester;
                    break;
            }

           
        }


    }


}