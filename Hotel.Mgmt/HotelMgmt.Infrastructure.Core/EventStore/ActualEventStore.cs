using System;
using System.Collections.Generic;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using HotelMgmt.Domain;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core.EventStore
{
    public class ActualEventStore : IStoreEvents, IDisposable
    {
        private readonly IDispatchEvents _eventsDispatcher;
        private readonly ISerializeEvent _eventSerializer;
        private readonly IEventStoreConnection _connection;

        private readonly UserCredentials _credentials;  
        public ActualEventStore(IDispatchEvents eventsDispatcher, ISerializeEvent eventSerializer, UserCredentials credentials)
        {
            _eventsDispatcher = eventsDispatcher;
            _eventSerializer = eventSerializer;
            _connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            _connection.ConnectAsync().GetAwaiter().GetResult();
            _credentials = credentials;
        }

        public void AddEventsToStream(Guid aggregateId, IList<IEvent> events)
        {
            var eventsData = new List<EventData>();
            foreach (var @event in events)
            {
                eventsData.Add(_eventSerializer.Serialize(@event));

                _connection.AppendToStreamAsync(GetStreamNameFormAggregateId(aggregateId), ExpectedVersion.Any, eventsData).GetAwaiter().GetResult();
            }

            _eventsDispatcher.Dispatch(events);
        }

        public EventStream GetStreamForAggregate(Guid aggregateId)
        {
            var streamName = GetStreamNameFormAggregateId(aggregateId);
            var serializedEvents = _connection.ReadStreamEventsForwardAsync(streamName, 0, 1000, false, _credentials).GetAwaiter().GetResult();
            var deserializeEvents = new EventStream(aggregateId);

            if (serializedEvents.Status == SliceReadStatus.StreamNotFound ||
                serializedEvents.Status == SliceReadStatus.StreamDeleted)
                return deserializeEvents;

            foreach (var serializedEvent in serializedEvents.Events)
            {
                var deserializedEvent = DeserializedEvent(serializedEvent);
                deserializeEvents.AppendEventToStream(deserializedEvent);
            }

            return deserializeEvents;

            Type GetTypeFromName(string typeName)
            {
                return Type.GetType(typeName);
            }

            IEvent DeserializedEvent(ResolvedEvent serializedEvent)
            {
                var eventType = GetTypeFromName(serializedEvent.Event.EventType);
                return _eventSerializer.Deserialize(serializedEvent);
            }
        }

        private string GetStreamNameFormAggregateId(Guid aggregateId) => $"myaggregate-[{aggregateId}]";

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}