using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI.SystemData;
using HotelMgmt.Domain.Abstractions;
using HotelMgmt.Domain.CleaningRequest;
using HotelMgmt.Domain.Rooms;
using HotelMgmt.Infrastructure;
using HotelMgmt.Infrastructure.Core;
using HotelMgmt.Infrastructure.Core.EventStore;
using HotelMgmt.Projections;



namespace HotelMgmt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating the commands");

            var rooms = new RoomReferential();
            var commands = new List<ICommand>();
            var rounds = 1;
            var roomsCount = 1;
            //var roomsCount = rooms.Count();
            for (int round = 1; round <= rounds; round++)
            {
                commands.AddRange(
                    Enumerable.Range(1, roomsCount)
                    .Select( roomNumber => new CleaningRequestCommand() {RequestedAt = DateTime.UtcNow, RoomId = rooms.GetByNumber(roomNumber).Id})
                    );
            }


            Console.WriteLine("Dispatching the commands");
            foreach (var command in commands)
            {
                CompositionRoot.DispatchCommand(command);
            }
            

            Console.ReadLine();

         
        }
    }

    public static class CompositionRoot
    {
        private static readonly IDispatchCommands CommandDispatcher;
        static CompositionRoot()
        {
            var eventDispatcher = new LocalEventDispatcher();
            var eventStore = new ActualEventStore(
                                                    eventDispatcher, 
                                                    new EventSerializer(),
                                                    new UserCredentials("admin", "changeit"));

            var roomRepo = new RoomRepository(eventStore);

            eventDispatcher.Subscribe(new CleaningRequestReportMessageHandler());

            CommandDispatcher = new SingleProcessCommandDispatcher();
            CommandDispatcher.Subscribe(new CleaningRequestHandler(roomRepo));
        }

        public static void DispatchCommand(ICommand command)
        {
            CommandDispatcher.Dispatch(command);
        }
    }
}