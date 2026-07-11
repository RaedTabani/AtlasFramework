using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Dev.Hub.Models;
using DeviGames.Atlas.Gameplay.Inventory.Services;

namespace DeviGames.Atlas.Dev.Hub.Services
{
    public sealed class DevHubSnapshotService
    {
        private readonly MissionService _missionService;
        private readonly ObjectiveService _objectiveService;
        private readonly MissionProgressService _progressService;
        private readonly InventoryService _inventoryService;

        public DevHubSnapshotService(
            MissionService missionService,
            ObjectiveService objectiveService,
            MissionProgressService progressService,
            InventoryService inventoryService)
        {
            _missionService = missionService;
            _objectiveService = objectiveService;
            _progressService = progressService;
            _inventoryService = inventoryService;
        }

        public DevHubSnapshot CreateSnapshot()
        {
            var snapshot = new DevHubSnapshot
            {
                HasActiveMission = _missionService.HasActiveMission,
                CurrentMissionId =
                    _missionService.CurrentMission != null
                        ? _missionService.CurrentMission.MissionId
                        : string.Empty
            };

            snapshot.InventoryItemIds.AddRange(
                _inventoryService.ItemIds);

            snapshot.CompletedMissionIds.AddRange(
                _progressService.CompletedMissionIds);

            foreach (var pair in _objectiveService.States)
            {
                var state = pair.Value;

                snapshot.Objectives.Add(
                    new ObjectiveSnapshot
                    {
                        ObjectiveId = state.ObjectiveId,
                        CurrentValue = state.CurrentValue,
                        TargetValue = state.TargetValue,
                        IsCompleted = state.IsCompleted
                    });
            }

            return snapshot;
        }
    }
}