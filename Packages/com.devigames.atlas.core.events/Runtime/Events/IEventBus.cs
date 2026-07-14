using System;

namespace DeviGames.Atlas.Core.Events
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> listener);

        void Unsubscribe<T>(Action<T> listener);

        void Publish<T>(T eventData);
        void AddObserver(IEventObserver observer);
        void RemoveObserver(IEventObserver observer);
    }
}