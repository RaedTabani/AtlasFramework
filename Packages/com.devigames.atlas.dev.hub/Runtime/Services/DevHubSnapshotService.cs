using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Runtime;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;

using DeviGames.Atlas.Dev.Hub.Models;

using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;

namespace DeviGames.Atlas.Dev.Hub.Services
{
    public sealed class DevHubSnapshotService
    {
        private readonly IMissionCollection _missionCollection;

        private readonly IObjectiveCollection _objectiveCollection;

        private readonly MissionProgressService _progressService;

        private readonly IInventoryService _inventoryService;

        private readonly IUnlockService _unlockService;
        private readonly ICurrencyService _currencyService;

        public DevHubSnapshotService(
            IMissionCollection missionCollection,
            IObjectiveCollection objectiveCollection,
            MissionProgressService progressService,
            IInventoryService inventoryService,
            IUnlockService unlockService,
            ICurrencyService currencyService)
        {
            _missionCollection =
                missionCollection
                ?? throw new ArgumentNullException(
                    nameof(missionCollection));

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

            _unlockService =
                unlockService
                ?? throw new ArgumentNullException(
                    nameof(unlockService));
                
            _currencyService =
                currencyService
                ?? throw new ArgumentNullException(nameof(currencyService));
        }

        public DevHubSnapshot CreateSnapshot()
        {
            var snapshot =
                new DevHubSnapshot();

            AddInventorySnapshot(
                snapshot);

            AddProgressSnapshot(
                snapshot);

            AddObjectiveSnapshots(
                snapshot);

            AddMissionSnapshots(
                snapshot);

            AddUnlockedSnapshot(
                snapshot);

            AddCurrencySnapshot(snapshot);

            return snapshot;
        }

        private void AddInventorySnapshot(
            DevHubSnapshot snapshot)
        {
            snapshot.InventoryItemIds.AddRange(
                _inventoryService.ItemIds);
        }

        private void AddProgressSnapshot(
            DevHubSnapshot snapshot)
        {
            snapshot.CompletedMissionIds.AddRange(
                _progressService.CompletedMissionIds);
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

                        DisplayName =
                            runtime.DisplayName,

                        CurrentValue =
                            runtime.CurrentValue,

                        TargetValue =
                            runtime.TargetValue,

                        IsCompleted =
                            runtime.IsCompleted
                    });
            }
        }

        private void AddMissionSnapshots(
            DevHubSnapshot snapshot)
        {
            var missions =
                _missionCollection.Missions;

            for (int index = 0;
                 index < missions.Count;
                 index++)
            {
                MissionRuntime runtime =
                    missions[index];

                snapshot.Missions.Add(
                    new MissionSnapshot
                    {
                        MissionId =
                            runtime.Id,

                        DisplayName =
                            runtime.DisplayName,

                        CompletedObjectiveCount =
                            runtime.CompletedObjectiveCount,

                        ObjectiveCount =
                            runtime.ObjectiveCount,

                        IsCompleted =
                            runtime.IsCompleted
                    });
            }
        }

        private void AddUnlockedSnapshot(
            DevHubSnapshot snapshot)
        {
            snapshot.UnlockedIds.AddRange(
                _unlockService.UnlockedIds);
        }

        private void AddCurrencySnapshot(
            DevHubSnapshot snapshot)
        {
            foreach (KeyValuePair<string, int> pair in _currencyService.Balances)
            {
                snapshot.Currencies.Add(
                    new CurrencySnapshot(
                        pair.Key,
                        pair.Value));
            }

            snapshot.Currencies.Sort(
                (left, right) =>
                    string.Compare(
                        left.CurrencyId,
                        right.CurrencyId,
                        StringComparison.Ordinal));
        }
    }
}