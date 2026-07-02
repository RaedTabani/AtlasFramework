using System;

namespace DeviGames.Atlas.Core.Events
{
    /// <summary>
    /// Entry point for publishing and subscribing to events.
    /// </summary>
    public static class EventBus
    {
        public static void Subscribe<T>(Action<T> listener)
        {
            EventBusProvider.Current.Subscribe(listener);
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            EventBusProvider.Current.Unsubscribe(listener);
        }

        public static void Publish<T>(T eventData)
        {
            EventBusProvider.Current.Publish(eventData);
        }

    }
}