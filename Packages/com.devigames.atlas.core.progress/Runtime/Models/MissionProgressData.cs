using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Progress.Models
{
    [Serializable]
    public sealed class MissionProgressData
    {
        public List<string> CompletedMissionIds = new();

        public MissionProgressData()
        {
        }
        public MissionProgressData(List<string> completedMissionIds)
        {
            CompletedMissionIds = completedMissionIds ?? new List<string>();
        }
        public bool IsCompleted(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
                return false;

            return CompletedMissionIds.Contains(missionId);
        }

        public bool MarkCompleted(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
                return false;

            if (CompletedMissionIds.Contains(missionId))
                return false;

            CompletedMissionIds.Add(missionId);
            return true;
        }

        public void Clear()
        {
            CompletedMissionIds.Clear();
        }
    }
}