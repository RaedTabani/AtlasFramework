using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Gameplay.WorldState.Interfaces;
using DeviGames.Atlas.Gameplay.WorldState.Models;

namespace DeviGames.Atlas.Gameplay.WorldState.Services
{
    public sealed class WorldStateSaveCoordinator :
        ISaveParticipant
    {
        private readonly IWorldStateService _worldStateService;
        private readonly SaveService _saveService;

        public string Key => "world-state";

        public WorldStateSaveCoordinator(
            IWorldStateService worldStateService,
            SaveService saveService)
        {
            _worldStateService = worldStateService ?? throw new ArgumentNullException(nameof(worldStateService));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
        }

        public async Task SaveAsync()
        {
            WorldStateData data =
                _worldStateService.CreateSnapshot();

            await _saveService.SaveAsync(Key, data);
        }

        public async Task LoadAsync()
        {
            bool exists =
                await _saveService.ExistsAsync(Key);

            if (!exists)
            {
                return;
            }

            WorldStateData data =
                await _saveService.LoadAsync<WorldStateData>(Key);

            _worldStateService.Load(data);
        }
    }
}