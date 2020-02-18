using System;
using System.Collections.Generic;
using HotelMgmt.Domain;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core.EventStore
{
    public interface IStoreEvents
    {
        void AddEventsToStream(Guid aggreggateId, IList<IEvent> events);
        EventStream GetStreamForAggregate(Guid aggregateId);
    }
}