using System;

namespace HotelMgmt.Domain.Abstractions
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
    }
}