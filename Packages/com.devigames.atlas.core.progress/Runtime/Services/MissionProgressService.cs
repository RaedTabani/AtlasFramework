using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;

namespace DeviGames.Atlas.Core.Progress.Services
{
    public sealed class MissionProgressService: IInitializable, IShutdownable
    {
        private MissionProgressData _data = new();

        public int CompletedMissionCount => _data.CompletedMissionIds.Count;

        public IReadOnlyCollection<string> CompletedMissionIds => _data.CompletedMissionIds;

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        public bool IsCompleted(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
                return false;

            return _data.CompletedMissionIds.Contains(missionId);
        }

        public void LoadProgress(MissionProgressData data)
        {
            _data = data ?? new MissionProgressData();
        }

        public MissionProgressData CreateSnapshot()
        {
            return new MissionProgressData
            {
                CompletedMissionIds = new HashSet<string>(_data.CompletedMissionIds)
            };
        }

        private void OnMissionCompleted(MissionCompletedEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.MissionId))
                return;

            _data.CompletedMissionIds.Add(e.MissionId);
        }
    }
}