using System.Collections.Generic;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public interface IDispatchEvents
    {
        void Subscribe<T>(IHandleEvents<T> eventHandler) where T : IEvent;
        void Dispatch(IList<IEvent> events);
    }
}