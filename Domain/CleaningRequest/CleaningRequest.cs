using System;

namespace HotelMgmt.Domain.CleaningRequest
{

    public class CleaningRequestHandler : IHandleCommand<CleaningRequestCommand>
    {
        private readonly IRepo<Room> _repo;

        public CleaningRequestHandler(IRepo<Room> repo)
        {
            _repo = repo;
        }

        public void RequestCleaning(CleaningRequestCommand cleaningRequest)
        {
            var room = _repo.GetById(cleaningRequest.RoomId);
            room.RequestCleaning(cleaningRequest.RequestedAt);
            _repo.Save(room);

        }

        public void Handle(CleaningRequestCommand command)
        {
            RequestCleaning(command);
        }
    }



    public class CleaningRequestCommand : ICommand
    {
        public int RoomId { get; set; }
        public DateTime RequestedAt { get; set; }
    }


    public class CleaningRequested : IEvent
    {
        public int RoomNumber { get; }
        public string Requester { get; }
        public DateTime RequestedAt { get; }

        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public CleaningRequested(int roomNumber, string requester, DateTime requestedAt)
        {
            RequestedAt = requestedAt;
            RoomNumber = roomNumber;
            Requester = requester;

            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
}
