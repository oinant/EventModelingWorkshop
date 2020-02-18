using System;
using System.Collections.Generic;

namespace HotelMgmt.Domain.Abstractions
{
    public class EventStream
    {
        public Guid Id { get; }
        private readonly List<IEvent> _stream;

        public EventStream(Guid id)
        {
            Id = id;
            _stream = new List<IEvent>();
        }

        public void AppendEventToStream(IEvent @event)
        {
            if (@event != null)
                _stream.Add(@event);
        }

        public IList<IEvent> GetEvents() => _stream.AsReadOnly();
    }
}