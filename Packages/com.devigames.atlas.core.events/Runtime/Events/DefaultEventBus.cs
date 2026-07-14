using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Events
{
    internal sealed class DefaultEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new();
        private readonly List<IEventObserver> _observers = new();

        public void Subscribe<T>(Action<T> listener)
        {
            var eventType = typeof(T);

            if (!_subscriptions.TryGetValue(eventType, out var listeners))
            {
                listeners = new List<Delegate>();
                _subscriptions.Add(eventType, listeners);
            }

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unsubscribe<T>(Action<T> listener)
        {
            var eventType = typeof(T);

            // 1. Check if anyone is even subscribed to this event type
            if (_subscriptions.TryGetValue(eventType, out var listeners))
            {
                // 2. Remove this specific listener from the list
                listeners.Remove(listener);

                // 3. Clean up: If the list is now completely empty, remove the event type from the dictionary
                if (listeners.Count == 0)
                {
                    _subscriptions.Remove(eventType);
                }
            }
        }
        public void Publish<T>(T eventData)
        {
            NotifyObservers(eventData);

            Type eventType = typeof(T);

            if (!_subscriptions.TryGetValue(
                    eventType,
                    out List<Delegate> listeners))
            {
                return;
            }

            var snapshot = new List<Delegate>(listeners);

            foreach (Delegate listener in snapshot)
            {
                ((Action<T>)listener)(eventData);
            }
        }
        public void AddObserver(IEventObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IEventObserver observer)
        {
            if (observer == null)
                return;

            _observers.Remove(observer);
        }

        private void NotifyObservers<T>(T eventData)
        {
            if (_observers.Count == 0)
                return;

            var snapshot =
                new List<IEventObserver>(_observers);

            foreach (IEventObserver observer in snapshot)
            {
                observer.OnEventPublished(
                    typeof(T),
                    eventData);
            }
        }

        internal void Clear()
        {
            _subscriptions.Clear();
        }
    }
}