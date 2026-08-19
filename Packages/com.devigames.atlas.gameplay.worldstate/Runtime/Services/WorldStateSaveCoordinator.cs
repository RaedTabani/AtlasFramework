using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Save.Services;

using DeviGames.Atlas.Gameplay.WorldState.Interfaces;
using DeviGames.Atlas.Gameplay.WorldState.Models;
using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Services
{
    public sealed class WorldStateSaveCoordinator
    {
        private const string SaveKey =
            "world-state";

        private readonly IWorldStateService
            _worldStateService;

        private readonly SaveService
            _saveService;

        public WorldStateSaveCoordinator(
            IWorldStateService worldStateService,
            SaveService saveService)
        {
            _worldStateService =
                worldStateService
                ?? throw new ArgumentNullException(
                    nameof(worldStateService));

            _saveService =
                saveService
                ?? throw new ArgumentNullException(
                    nameof(saveService));
        }

        public async Task SaveAsync()
        {
            WorldStateData data =
                _worldStateService.CreateSnapshot();

            await _saveService.SaveAsync(
                SaveKey,
                data);
        }

        public async Task LoadAsync()
        {
            bool exists =
                await _saveService.ExistsAsync(
                    SaveKey);

            if (!exists)
            {
                return;
            }

            WorldStateData data =
                await _saveService.LoadAsync<
                    WorldStateData>(
                    SaveKey);

            _worldStateService.Load(
                data);
        }
    }
}