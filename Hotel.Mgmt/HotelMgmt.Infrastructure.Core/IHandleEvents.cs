using HotelMgmt.Domain;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public interface IHandleEvents<T> where T : IEvent
    {
        void HandleEvent(T @event);
    }
}