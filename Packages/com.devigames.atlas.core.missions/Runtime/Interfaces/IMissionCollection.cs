using System.Collections.Generic;
using DeviGames.Atlas.Core.Missions.Runtime;

namespace DeviGames.Atlas.Core.Missions.Interfaces
{
    public interface IMissionCollection
    {
        IReadOnlyList<MissionRuntime> Missions { get; }

        int Count { get; }

        void Add(
            MissionRuntime mission);

        bool Remove(
            MissionRuntime mission);

        bool Contains(
            MissionRuntime mission);

        bool TryGet(
            string missionId,
            out MissionRuntime mission);

        MissionRuntime Get(
            string missionId);

        void Clear();
    }
}