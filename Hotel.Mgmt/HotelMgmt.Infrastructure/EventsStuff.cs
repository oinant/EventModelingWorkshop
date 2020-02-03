using System;
using System.Collections.Generic;
using HotelMgmt.Domain;

namespace HotelMgmt.Infrastructure
{
    public interface IHandleEvents
    {
        void HandleEvent(IEvent @event);
    }

    public interface IDispatchEvents
    {
        void Subscribe(IHandleEvents eventHandler, Type ofEventIHandle);
        void Dispatch(IList<IEvent> events);
    }

    public class LocalEventDispatcher : IDispatchEvents
    {
        private Dictionary<Type, List<IHandleEvents>> _registry;

        public LocalEventDispatcher()
        {
            _registry = new Dictionary<Type, List<IHandleEvents>>();
        }

        public void Subscribe(IHandleEvents eventHandler, Type ofEventIHandle)
        {
            if (!_registry.ContainsKey(ofEventIHandle))
                _registry.Add(ofEventIHandle, new List<IHandleEvents>());

            var handlers = _registry[ofEventIHandle];
            handlers.Add(eventHandler);
        }

        public void Dispatch(IList<IEvent> events)
        {
            foreach (var @event in events)
            {
                if (_registry.ContainsKey(@event.GetType()))
                {
                    var handlers = _registry[@event.GetType()];
                    foreach (var handler in handlers)
                    {
                        handler.HandleEvent(@event);
                    }
                }
            }
        }
    }
}