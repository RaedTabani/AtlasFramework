using System;

namespace DeviGames.Atlas.Core.Events
{
    public interface IEventObserver
    {
        void OnEventPublished(
            Type eventType,
            object eventData);
    }
}