using System;

using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Runtime;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Dev.Hub.Models;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;

namespace DeviGames.Atlas.Dev.Hub.Services
{
    public sealed class DevHubSnapshotService
    {
        private readonly MissionService _missionService;

        private readonly IObjectiveCollection
            _objectiveCollection;

        private readonly MissionProgressService
            _progressService;

        private readonly IInventoryService
            _inventoryService;

        public DevHubSnapshotService(
            MissionService missionService,
            IObjectiveCollection objectiveCollection,
            MissionProgressService progressService,
            IInventoryService inventoryService)
        {
            _missionService =
                missionService
                ?? throw new ArgumentNullException(
                    nameof(missionService));

            _objectiveCollection =
                objectiveCollection
                ?? throw new ArgumentNullException(
                    nameof(objectiveCollection));

            _progressService =
                progressService
                ?? throw new ArgumentNullException(
                    nameof(progressService));

            _inventoryService =
                inventoryService
                ?? throw new ArgumentNullException(
                    nameof(inventoryService));
        }

        public DevHubSnapshot CreateSnapshot()
        {
            var snapshot =
                new DevHubSnapshot
                {
                    HasActiveMission =
                        true,

                    CurrentMissionId =
                        _missionService.CurrentMission != null
                            ? _missionService
                                .CurrentMission
                            : string.Empty
                };

            snapshot.InventoryItemIds.AddRange(
                _inventoryService.ItemIds);

            snapshot.CompletedMissionIds.AddRange(
                _progressService.CompletedMissionIds);

            AddObjectiveSnapshots(
                snapshot);

            return snapshot;
        }

        private void AddObjectiveSnapshots(
            DevHubSnapshot snapshot)
        {
            var objectives =
                _objectiveCollection.Objectives;

            for (int index = 0;
                 index < objectives.Count;
                 index++)
            {
                ObjectiveRuntime runtime =
                    objectives[index];

                snapshot.Objectives.Add(
                    new ObjectiveSnapshot
                    {
                        ObjectiveId =
                            runtime.Id,

                        CurrentValue =
                            runtime.CurrentValue,

                        TargetValue =
                            runtime.TargetValue,

                        IsCompleted =
                            runtime.IsCompleted
                    });
            }
        }
    }
}