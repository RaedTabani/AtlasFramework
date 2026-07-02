using System;

namespace DeviGames.Atlas.Core.Events
{
    /// <summary>
    /// Entry point for publishing and subscribing to events.
    /// </summary>
    public static class EventBus
    {
        private static readonly IEventBus _bus = new DefaultEventBus();
        public static void Subscribe<T>(Action<T> listener)
        {
            _bus.Subscribe(listener);
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            _bus.Unsubscribe(listener);
        }

        public static void Publish<T>(T eventData)
        {
            _bus.Publish(eventData);
        }

        public static void Clear()
        {
            if (_bus is DefaultEventBus defaultBus)
            {
                defaultBus.Clear();
            }
        }
    }
}