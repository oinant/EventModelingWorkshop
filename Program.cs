using System;
using System.Collections.Generic;
using HotelMgmt.Domain;
using HotelMgmt.Domain.CleaningRequest;
using HotelMgmt.Infrastructure;
using HotelMgmt.Projections;

namespace HotelMgmt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CompositionRoot.Bootstrap();

            var myCommand = new CleaningRequestCommand() { RequestedAt = DateTime.UtcNow, RoomId = 12 };

            CompositionRoot.DispatchCommand(myCommand);

            Console.ReadLine();
        }
    }



    public static class CompositionRoot
    {
        private static RoomRepository _roomRepo;
        private static IDispatchCommands _commandDispatcher;

        public static void Bootstrap()
        {
            var eventDispatcher = new LocalEventDispatcher();
            //var eventStore = new InMemoryEventStore(eventDispatcher);
            var eventStore = new ActualEventStore(eventDispatcher);
            
            _roomRepo = new RoomRepository(eventStore);

            eventDispatcher.Subscribe(new CleaningRequestReportMessageHandler(), typeof(CleaningRequested));

            //_commandDispatcher = new SingleProcessCommandDispatcher();
            
            //var casted = (IHandleCommand<ICommand>) new CleaningRequestHandler(_roomRepo);
            //_commandDispatcher.Subscribe(casted, typeof(CleaningRequestCommand));
        }

        public static void DispatchCommand(ICommand command)
        {
            //_commandDispatcher.Dispatch(command);
            var handler = GetHandler();
            handler.Handle((CleaningRequestCommand)command);
        }

        public static CleaningRequestHandler GetHandler()
        {
            return new CleaningRequestHandler(_roomRepo);
        }

    }


}