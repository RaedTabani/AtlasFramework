using System;

namespace DeviGames.Atlas.Core.Progress.Models
{
    [Serializable]
    public sealed class MissionProgressData
    {
        public string[] CompletedMissionIds =
            System.Array.Empty<string>();
    }
}