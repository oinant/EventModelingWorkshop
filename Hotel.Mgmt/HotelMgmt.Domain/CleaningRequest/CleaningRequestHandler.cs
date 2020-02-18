using HotelMgmt.Domain.Abstractions;

namespace HotelMgmt.Domain.CleaningRequest
{

    public class CleaningRequestHandler : IHandleCommand<CleaningRequestCommand>
    {
        private readonly IRepo<RoomCleaning> _repo;

        public CleaningRequestHandler(IRepo<RoomCleaning> repo)
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
}
