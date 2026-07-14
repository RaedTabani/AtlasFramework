using System;

namespace DeviGames.Atlas.Core.Diagnostics.Models
{
    public sealed class EventRecord
    {
        public DateTime TimestampUtc { get; }
        public string EventName { get; }
        public Type EventType { get; }
        public object EventData { get; }

        public EventRecord(
            DateTime timestampUtc,
            Type eventType,
            object eventData)
        {
            TimestampUtc = timestampUtc;
            EventType = eventType;
            EventName = eventType?.Name ?? "UnknownEvent";
            EventData = eventData;
        }
    }
}