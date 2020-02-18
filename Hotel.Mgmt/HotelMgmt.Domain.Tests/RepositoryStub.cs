using System;
using System.Collections.Generic;
using System.Linq;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Domain.Tests
{
    public class RepositoryStub<T> : IRepo<T> where T : class, IAggregate
    {
        private readonly T _aggregate;

        public IList<IEvent> ProducedEvents { get; private set; }

        public RepositoryStub(T aggregate)
        {
            _aggregate = aggregate;
        }
        
        public T GetById(Guid id)
        {
            return _aggregate;
        }

        public void Save(T aggregate)
        {
            ProducedEvents = aggregate.Produced.Select(x=> x).ToList();
            aggregate.CommitEvents();
        }
    }
}