namespace HotelMgmt.Domain.Abstractions
{
    public interface IHandleCommand<in T> where T : ICommand
    {
        void Handle(T command);
    }
}