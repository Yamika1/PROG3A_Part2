namespace Prog_part_2.Models
{
    public class NotificationSystem : IServiceRequest
    {
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void EnableNotifications(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Notify(message);
            }
        }
    }
}