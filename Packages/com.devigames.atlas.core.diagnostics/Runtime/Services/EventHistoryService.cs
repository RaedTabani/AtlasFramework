using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Diagnostics.Models;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;

namespace DeviGames.Atlas.Core.Diagnostics.Services
{
    public sealed class EventHistoryService :
        IInitializable,
        IShutdownable,
        IEventObserver
    {
        private readonly List<EventRecord> _records = new();
        private readonly int _capacity;

        private long _nextSequenceNumber;
        private bool _isPaused;

        public IReadOnlyList<EventRecord> Records => _records;
        public int Count => _records.Count;
        public int Capacity => _capacity;

        public bool IsPaused
        {
            get => _isPaused;
            set => _isPaused = value;
        }

        public EventHistoryService(int capacity = 250)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    "Event history capacity must be greater than zero.");
            }

            _capacity = capacity;
        }

        public void Initialize()
        {
            EventBus.AddObserver(this);
        }

        public void Shutdown()
        {
            EventBus.RemoveObserver(this);
        }

        public void OnEventPublished(
            Type eventType,
            object eventData)
        {
            if (_isPaused)
                return;

            if (_records.Count >= _capacity)
            {
                _records.RemoveAt(0);
            }

            _records.Add(
                new EventRecord(
                    ++_nextSequenceNumber,
                    DateTime.UtcNow,
                    eventType,
                    eventData));
        }

        public void Clear()
        {
            _records.Clear();
        }
    }
}