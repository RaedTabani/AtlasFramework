using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Gameplay.Objectives.Models;
using DeviGames.Atlas.Gameplay.Objectives.Services;

namespace DeviGames.Atlas.Gameplay.Objectives.Content
{
    public sealed class GameplayObjectiveContentInstaller: IContentPackageConsumer
    {
        public int Order => 200;
        private readonly GameplayObjectiveAdapter
            _adapter;

        public GameplayObjectiveContentInstaller(
            GameplayObjectiveAdapter adapter)
        {
            _adapter =
                adapter
                ?? throw new ArgumentNullException(
                    nameof(adapter));
        }

        public void Install(
            ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(
                    nameof(package));
            }

            InstallItemCollectedBindings(
                package);

            InstallAreaEnteredBindings(
                package);

            InstallDoorOpenedBindings(
                package);
        }

        private void InstallItemCollectedBindings(
            ContentPackageData package)
        {
            foreach (ItemCollectedObjectiveBindingData data
                     in package.ItemCollectedObjectiveBindings)
            {
                _adapter.AddItemCollectedObjectiveBinding(
                    new ItemCollectedObjectiveBinding(
                        objectiveId:
                            data.ObjectiveId,
                        itemId:
                            data.ItemId,
                        progressAmount:
                            data.ProgressAmount));
            }
        }

        private void InstallAreaEnteredBindings(
            ContentPackageData package)
        {
            foreach (AreaEnteredObjectiveBindingData data
                     in package.AreaEnteredObjectiveBindings)
            {
                _adapter.AddAreaEnteredObjectiveBinding(
                    new AreaEnteredObjectiveBinding(
                        objectiveId:
                            data.ObjectiveId,
                        areaId:
                            data.AreaId,
                        progressAmount:
                            data.ProgressAmount));
            }
        }

        private void InstallDoorOpenedBindings(
            ContentPackageData package)
        {
            foreach (DoorOpenedObjectiveBindingData data
                     in package.DoorOpenedObjectiveBindings)
            {
                _adapter.AddDoorOpenedObjectiveBinding(
                    new DoorOpenedObjectiveBinding(
                        objectiveId:
                            data.ObjectiveId,
                        doorId:
                            data.DoorId,
                        progressAmount:
                            data.ProgressAmount));
            }
        }
    }
}