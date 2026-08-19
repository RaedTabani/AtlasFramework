using System;

using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Content.Models;

using DeviGames.Atlas.Gameplay.WorldState.Models;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Content
{
    public sealed class WorldStateContentInstaller :
        IContentPackageConsumer
    {
        private readonly GameplayWorldStateAdapter
            _adapter;

        public int Order =>
            250;

        public WorldStateContentInstaller(
            GameplayWorldStateAdapter adapter)
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

            foreach (
                DoorOpenedWorldStateBindingData data
                in package.DoorOpenedWorldStateBindings)
            {
                _adapter.AddDoorOpenedBinding(
                    new DoorOpenedWorldStateBinding(
                        doorId:
                            data.DoorId,
                        stateKey:
                            data.StateKey,
                        value:
                            data.Value));
            }
        }
    }
}