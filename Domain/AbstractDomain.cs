using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HotelMgmt.Domain
{

    public interface ICommand { }


    public interface IHandleCommand<T> where T : ICommand
    {
        void Handle(T command);
    }

    
   
    public interface IRepo<T> where T : class, IAggregate
    {
        T GetById(int id);
        void Save(T aggregate);
    }

    public interface IAggregate
    {
        IList<IEvent> Produced { get; }


        void Apply(IList<IEvent> events);

        void CommitEvents();
    }

    public abstract class Aggregate : IAggregate
    {
        public IList<IEvent> Produced { get; }

        protected Aggregate(IList<IEvent> pastStream)
        {
            Produced = new List<IEvent>();

            foreach (var @event in pastStream)
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

    public interface IEvent
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
    }

    public abstract class Event : IEvent, IEquatable<Event>
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                return Utf8Json.JsonSerializer.ToJsonString(this).GetHashCode();
                //return (Id.GetHashCode() * 397) ^ CreatedAt.GetHashCode();
            }
        }

        public bool Equals(Event other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && CreatedAt.Equals(other.CreatedAt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Event) obj);
        }
    }
}
