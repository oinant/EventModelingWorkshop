using EventStore.ClientAPI;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public interface ISerializeEvent
    {
        EventData Serialize(IEvent @event);
        IEvent Deserialize(ResolvedEvent resolvedEvent);
    }
}