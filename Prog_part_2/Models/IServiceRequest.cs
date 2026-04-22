namespace Prog_part_2.Models
{
    public interface IServiceRequest
    {
        void EnableNotifications(IObserver observer);
        void NotifyObservers(string message);
    }

}
