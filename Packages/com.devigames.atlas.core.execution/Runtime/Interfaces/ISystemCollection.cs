using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Execution.Interfaces
{
    public interface ISystemCollection
    {
        IReadOnlyList<object> Systems { get; }

        int Count { get; }

        void Add(object system);

        bool Remove(object system);

        bool Contains(object system);

        void Clear();
    }
}