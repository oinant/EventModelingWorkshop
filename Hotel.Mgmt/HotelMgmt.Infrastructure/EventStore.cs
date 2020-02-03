using System;
using System.Collections.Generic;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using HotelMgmt.Domain;
using HotelMgmt.Domain.CleaningRequest;

namespace HotelMgmt.Infrastructure
{
    public interface IStoreEvents
    {
        void AddEventsToStream(int aggreggateId, IList<IEvent> events);
        IList<IEvent> GetStreamForAggregate(int aggregateId);
    }

    public class InMemoryEventStore : IStoreEvents
    {
        private readonly IDispatchEvents _eventsDispatcher;
        private IDictionary<int, EventStream> store;

        public InMemoryEventStore(IDispatchEvents eventsDispatcher)
        {
            _eventsDispatcher = eventsDispatcher;
            store = new Dictionary<int, EventStream>();
        }

        public void AddEventsToStream(int aggreggateId, IList<IEvent> events)
        {
            if (!store.ContainsKey(aggreggateId))
                store.Add(aggreggateId, new EventStream());

            var stream = store[aggreggateId];
            foreach (var @event in events)
            {
                stream.AppendEventToStream(@event);
            }
            _eventsDispatcher.Dispatch(events);

        }

        public IList<IEvent> GetStreamForAggregate(int aggregateId)
        {
            if (!store.ContainsKey(aggregateId))
                return new List<IEvent>();

            return store[aggregateId].GetEvents();
        }

        public class EventStream
        {
            //private int _id;
            private List<IEvent> _stream;

            public EventStream()
            {
                //_id = 1;
                _stream = new List<IEvent>();
            }

            public void AppendEventToStream(IEvent @event)
            {
                _stream.Add(@event);
            }

            public IList<IEvent> GetEvents() => _stream.AsReadOnly();

        }
    }

    public class ActualEventStore : IStoreEvents, IDisposable
    {
        private readonly IDispatchEvents _eventsDispatcher;
        private IEventStoreConnection _connection;

        UserCredentials credentials => new UserCredentials("admin", "changeit");
 
        public ActualEventStore(IDispatchEvents eventsDispatcher)
        {
            _eventsDispatcher = eventsDispatcher;
            _connection = EventStore.ClientAPI.EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            _connection.ConnectAsync().GetAwaiter().GetResult();
        }

        public void AddEventsToStream(int aggreggateId, IList<IEvent> events)
        {
            foreach (var @event in events)
            {
                EventData eventData = new EventData(Guid.NewGuid(), @event.GetType().FullName, true, Utf8Json.JsonSerializer.Serialize(@event), null);
                _connection.AppendToStreamAsync($"myaggregate-[{aggreggateId}]", ExpectedVersion.Any, eventData).GetAwaiter().GetResult();
            }
            _eventsDispatcher.Dispatch(events);
        }

        public IList<IEvent> GetStreamForAggregate(int aggregateId)
        {
            var serializedEvents = _connection.ReadAllEventsForwardAsync(Position.Start, 1000, false, credentials).GetAwaiter().GetResult();
            var deserializeEvents = new List<IEvent>();
            foreach (var serializeEvent in serializedEvents.Events)
            {
                var eventType = GetTypeFromName(serializeEvent.Event.EventType);

                if(eventType == typeof(CleaningRequested))
                    deserializeEvents.Add(Utf8Json.JsonSerializer.Deserialize<CleaningRequested>(serializeEvent.Event.Data));
            }

            return deserializeEvents;

            Type GetTypeFromName(string typeName)
            {
                return Type.GetType(typeName);
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}