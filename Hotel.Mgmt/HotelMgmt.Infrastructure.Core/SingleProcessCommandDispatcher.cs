using System;
using System.Collections.Generic;
using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Infrastructure.Core
{
    public class SingleProcessCommandDispatcher : IDispatchCommands
    {
        private readonly Dictionary<Type, Action<ICommand>> _registry;

        public SingleProcessCommandDispatcher()
        {
            _registry = new Dictionary<Type, Action<ICommand>>();
        }

        public void Subscribe<T>(IHandleCommand<T> commandHandler) where T : ICommand
        {
            if (!_registry.ContainsKey(typeof(T)))
                _registry.Add(typeof(T), command => commandHandler.Handle((T)command));
        }

        public void Dispatch(ICommand command)
        {
            if(_registry.ContainsKey(command.GetType()))
                _registry[command.GetType()](command);
        }
    }

}