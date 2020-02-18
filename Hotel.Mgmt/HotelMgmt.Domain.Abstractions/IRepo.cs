using System;

namespace HotelMgmt.Domain.Abstractions
{
    public interface IRepo<T> where T : class, IAggregate
    {
        T GetById(Guid id);
        void Save(T aggregate);
    }
}