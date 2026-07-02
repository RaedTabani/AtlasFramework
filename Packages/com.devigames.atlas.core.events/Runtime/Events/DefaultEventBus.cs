using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Events
{
    internal sealed class DefaultEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new();

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
            var eventType = typeof(T);

            // 1. If nobody is listening to this event, just exit safely.
            if (!_subscriptions.TryGetValue(eventType, out var listeners))
            {
                return;
            }

            // 2. Take a snapshot of the current listeners. 
            // Creating a new list prevents the "Collection was modified" crash if a listener unsubscribes during the loop.
            var listenersCopy = new List<Delegate>(listeners);

            // 3. Loop through the safe copy and invoke the methods
            foreach (var listener in listenersCopy)
            {
                // Cast the generic Delegate back to the specific Action<T> and execute it
                ((Action<T>)listener)(eventData);
            }
        }

        internal void Clear()
        {
            _subscriptions.Clear();
        }
    }
}