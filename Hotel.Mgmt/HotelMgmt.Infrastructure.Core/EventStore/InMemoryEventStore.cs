using System;
using System.Collections.Generic;
using HotelMgmt.Domain;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core.EventStore
{
    public class InMemoryEventStore : IStoreEvents
    {
        private readonly IDispatchEvents _eventsDispatcher;
        private IDictionary<Guid, EventStream> store;

        public InMemoryEventStore(IDispatchEvents eventsDispatcher)
        {
            _eventsDispatcher = eventsDispatcher;
            store = new Dictionary<Guid, EventStream>();
        }

        public void AddEventsToStream(Guid aggregateId, IList<IEvent> events)
        {
            if (!store.ContainsKey(aggregateId))
                store.Add(aggregateId, new EventStream(aggregateId));

            var stream = store[aggregateId];
            foreach (var @event in events)
            {
                stream.AppendEventToStream(@event);
            }
            _eventsDispatcher.Dispatch(events);

        }

        public EventStream GetStreamForAggregate(Guid aggregateId)
        {
            if (!store.ContainsKey(aggregateId))
                return new EventStream(aggregateId);

            return store[aggregateId];
        }


    }
}