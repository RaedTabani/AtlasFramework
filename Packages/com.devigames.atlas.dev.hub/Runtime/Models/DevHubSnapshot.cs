using System.Collections.Generic;

namespace DeviGames.Atlas.Dev.Hub.Models
{
    public sealed class DevHubSnapshot
    {
        public List<string> InventoryItemIds { get; } =
            new();

        public List<string> CompletedMissionIds { get; } =
            new();

        public List<ObjectiveSnapshot> Objectives { get; } =
            new();

        public List<MissionSnapshot> Missions { get; } =
            new();
    }
}