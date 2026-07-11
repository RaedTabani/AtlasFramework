using System.Collections.Generic;

namespace DeviGames.Atlas.Dev.Hub.Models
{
    public sealed class DevHubSnapshot
    {
        public string CurrentMissionId { get; set; }
        public bool HasActiveMission { get; set; }

        public List<string> InventoryItemIds { get; set; } = new();
        public List<string> CompletedMissionIds { get; set; } = new();
        public List<ObjectiveSnapshot> Objectives { get; set; } = new();
    }

    public sealed class ObjectiveSnapshot
    {
        public string ObjectiveId { get; set; }
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }
        public bool IsCompleted { get; set; }
    }
}