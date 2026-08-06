using System.Collections.Generic;
using DeviGames.Atlas.Core.Objectives.Runtime;

namespace DeviGames.Atlas.Core.Objectives.Interfaces
{
    public interface IObjectiveCollection
    {
        IReadOnlyList<ObjectiveRuntime> Objectives { get; }

        int Count { get; }

        void Add(
            ObjectiveRuntime objective);

        bool Remove(
            ObjectiveRuntime objective);

        bool Contains(
            ObjectiveRuntime objective);

        bool TryGet(
            string objectiveId,
            out ObjectiveRuntime objective);

        ObjectiveRuntime Get(
            string objectiveId);

        void Clear();
    }
}