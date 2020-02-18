using System;
using System.Collections.Generic;

namespace HotelMgmt.Domain.Abstractions
{
    public interface IAggregate
    {
        Guid Id { get; }
        
        IList<IEvent> Produced { get; }

        void Apply(IList<IEvent> events);

        void CommitEvents();
    }
}