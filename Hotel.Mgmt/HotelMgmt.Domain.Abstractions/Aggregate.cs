using System;
using System.Collections.Generic;

namespace HotelMgmt.Domain.Abstractions
{
    public abstract class Aggregate : IAggregate
    {
        public IList<IEvent> Produced { get; }
        
        public Guid Id { get; }

        protected Aggregate(EventStream pastStream)
        {
            Id = pastStream.Id;
            Produced = new List<IEvent>();

            foreach (var @event in pastStream.GetEvents())
            {
                Evolve(@event);
            }
        }

        protected abstract void Evolve(IEvent @event);

        protected void Apply(IEvent @event)
        {
            Evolve(@event);
            Produced.Add(@event);
        }

        public void Apply(IList<IEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        public void CommitEvents()
        {
            Produced.Clear();
        }
    }
}
