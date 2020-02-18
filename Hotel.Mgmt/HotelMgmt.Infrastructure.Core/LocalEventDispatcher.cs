using System;
using System.Collections.Generic;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public class LocalEventDispatcher : IDispatchEvents
    {
        private Dictionary<Type, List<Action<IEvent>>> _registry;

        public LocalEventDispatcher()
        {
            _registry = new Dictionary<Type, List<Action<IEvent>>>();
        }

        public void Subscribe<T>(IHandleEvents<T> eventHandler) where T : IEvent
        {
            if (!_registry.ContainsKey(typeof(T)))
                _registry.Add(typeof(T), new List<Action<IEvent>>());

            var handlers = _registry[typeof(T)];
            handlers.Add(@event => eventHandler.HandleEvent((T)@event));
        }

        public void Dispatch(IList<IEvent> events)
        {
            foreach (var @event in events)
            {
                if (!_registry.ContainsKey(@event.GetType())) 
                    continue;

                var handlers = _registry[@event.GetType()];
                foreach (var handler in handlers)
                {
                    handler(@event);
                }
            }
        }
    }
}