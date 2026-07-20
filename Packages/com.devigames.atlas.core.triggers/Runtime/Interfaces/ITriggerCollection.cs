using System.Collections.Generic;
using DeviGames.Atlas.Core.Triggers.Runtime;

namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerCollection
    {
        IReadOnlyList<TriggerRuntime> Triggers { get; }

        int Count { get; }

        void Add(TriggerRuntime trigger);

        bool Remove(TriggerRuntime trigger);

        bool Contains(TriggerRuntime trigger);

        bool TryGet(
            string id,
            out TriggerRuntime trigger);

        void Clear();
    }
}