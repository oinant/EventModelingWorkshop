using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public interface IDispatchCommands
    {
        void Subscribe<T>(IHandleCommand<T> commandHandler) where T : ICommand;
        void Dispatch(ICommand command);
    }
}