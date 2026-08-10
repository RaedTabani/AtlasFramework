using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;

namespace DeviGames.Atlas.Core.Missions.Collections
{
    public sealed class MissionCollection :
        IMissionCollection
    {
        private readonly List<MissionRuntime> _missions =
            new();

        private readonly Dictionary<string, MissionRuntime>
            _missionsById =
                new(StringComparer.Ordinal);

        public IReadOnlyList<MissionRuntime> Missions =>
            _missions;

        public int Count =>
            _missions.Count;

        public void Add(
            MissionRuntime mission)
        {
            if (mission == null)
            {
                throw new ArgumentNullException(
                    nameof(mission));
            }

            string missionId =
                mission.Id;

            if (_missionsById.ContainsKey(
                    missionId))
            {
                throw new InvalidOperationException(
                    $"A mission with ID '{missionId}' " +
                    "already exists.");
            }

            _missions.Add(
                mission);

            _missionsById.Add(
                missionId,
                mission);
        }

        public bool Remove(
            MissionRuntime mission)
        {
            if (mission == null)
                return false;

            if (!_missionsById.TryGetValue(
                    mission.Id,
                    out MissionRuntime existing))
            {
                return false;
            }

            if (!ReferenceEquals(
                    existing,
                    mission))
            {
                return false;
            }

            _missionsById.Remove(
                mission.Id);

            for (int index = 0;
                 index < _missions.Count;
                 index++)
            {
                if (!ReferenceEquals(
                        _missions[index],
                        mission))
                {
                    continue;
                }

                _missions.RemoveAt(
                    index);

                return true;
            }

            return false;
        }

        public bool Contains(
            MissionRuntime mission)
        {
            if (mission == null)
                return false;

            return _missionsById.TryGetValue(
                       mission.Id,
                       out MissionRuntime existing)
                   &&
                   ReferenceEquals(
                       existing,
                       mission);
        }

        public bool TryGet(
            string missionId,
            out MissionRuntime mission)
        {
            if (string.IsNullOrWhiteSpace(
                    missionId))
            {
                mission = null;

                return false;
            }

            return _missionsById.TryGetValue(
                missionId,
                out mission);
        }

        public MissionRuntime Get(
            string missionId)
        {
            if (string.IsNullOrWhiteSpace(
                    missionId))
            {
                throw new ArgumentException(
                    "Mission ID cannot be empty.",
                    nameof(missionId));
            }

            if (!_missionsById.TryGetValue(
                    missionId,
                    out MissionRuntime mission))
            {
                throw new KeyNotFoundException(
                    $"Mission '{missionId}' does not exist.");
            }

            return mission;
        }

        public void Clear()
        {
            _missions.Clear();
            _missionsById.Clear();
        }
    }
}