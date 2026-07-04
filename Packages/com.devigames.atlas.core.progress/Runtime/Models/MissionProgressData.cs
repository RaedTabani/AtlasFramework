using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Progress.Models
{
    public sealed class MissionProgressData
    {
        public HashSet<string> CompletedMissionIds { get; set; } = new();
    }
}