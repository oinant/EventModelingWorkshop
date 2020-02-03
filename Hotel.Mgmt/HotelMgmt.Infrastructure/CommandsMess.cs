using System;
using System.Collections.Generic;
using HotelMgmt.Domain;

namespace HotelMgmt.Infrastructure
{
    public interface IDispatchCommands
    {
        void Subscribe(IHandleCommand<ICommand> commandHandler, Type ofCommandIHandle);
        void Dispatch(ICommand command);
    }

    public class SingleProcessCommandDispatcher : IDispatchCommands
    {
        private Dictionary<Type, IHandleCommand<ICommand>> _registry;

        public SingleProcessCommandDispatcher()
        {
            _registry = new Dictionary<Type, IHandleCommand<ICommand>>();
        }

        public void Subscribe(IHandleCommand<ICommand> commandHandler, Type ofCommandIHandle)
        {
            if (!_registry.ContainsKey(ofCommandIHandle))
                _registry.Add(ofCommandIHandle, commandHandler);
        }

        public void Dispatch(ICommand command)
        {
            if(_registry.ContainsKey(command.GetType()))
                _registry[command.GetType()].Handle(command);
        }
    }

}