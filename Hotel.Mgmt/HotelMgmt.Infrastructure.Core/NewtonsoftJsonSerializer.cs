using System;
using System.Text;
using EventStore.ClientAPI;
using HotelMgmt.Domain.Abstractions;
using Newtonsoft.Json;

namespace HotelMgmt.Infrastructure.Core
{
    public class EventSerializer : ISerializeEvent
    {
        public EventData Serialize(IEvent @event)
        {
            SerializeEventData(@event);

            var eventData = new EventData(
                                Guid.NewGuid(),
                                @event.GetType().FullName,
                                true,
                                SerializeEventData(@event),
                                SerializeEventMetadata(@event));

            return eventData;

            byte[] SerializeEventData(IEvent eventToSerialize)
            {
                var eventJson = JsonConvert.SerializeObject(eventToSerialize);
                return Encoding.ASCII.GetBytes(eventJson);
            }
            
            byte[] SerializeEventMetadata(IEvent eventToSerialize)
            {
                var metadata = new EventMetadata(eventToSerialize);
                return Encoding.ASCII.GetBytes(metadata.ToJson());
            }


        }

        public IEvent Deserialize(ResolvedEvent resolvedEvent)
        {
            var jsonMetadata = Encoding.ASCII.GetString(resolvedEvent.Event.Metadata);
            var metadata = EventMetadata.FromJson(jsonMetadata);
            
            var eventData = Encoding.ASCII.GetString(resolvedEvent.Event.Data);
            var deserialized = (IEvent)JsonConvert.DeserializeObject(eventData, metadata.EventType);
            return deserialized;
        }

        class EventMetadata
        {
            public readonly Type EventType;

            public EventMetadata(IEvent @event)
            {
                EventType = @event.GetType();
            }

            private EventMetadata(string eventType)
            {
                EventType = Type.GetType(eventType);
            }

            public static EventMetadata FromJson(string jsonMetadata)
            {
                var parsed = JsonConvert.DeserializeObject<StringifiedMetadata>(jsonMetadata);
                return new EventMetadata(parsed.EventType);
            }

            public string ToJson()
            {
                var stringifiedMetadata = new StringifiedMetadata()
                {
                    EventType = this.EventType.AssemblyQualifiedName
                };

                return JsonConvert.SerializeObject(stringifiedMetadata);
            }

            internal class StringifiedMetadata
            {
                public string EventType { get; set; }


            }
        }
    }
}