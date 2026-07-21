using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerConditionFactoryRegistry
    {
        IReadOnlyCollection<string> Types { get; }

        int Count { get; }

        void Register(
            ITriggerConditionFactory factory);

        bool Unregister(
            string type);

        bool Contains(
            string type);

        bool TryGet(
            string type,
            out ITriggerConditionFactory factory);

        ITriggerConditionFactory Get(
            string type);

        void Clear();
    }
}