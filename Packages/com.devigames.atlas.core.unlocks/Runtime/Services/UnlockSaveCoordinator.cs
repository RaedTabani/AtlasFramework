using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Models;
using DeviGames.Atlas.Core.Unlocks.Events;

namespace DeviGames.Atlas.Core.Unlocks.Services
{
    public sealed class UnlockSaveCoordinator : ISaveParticipant
    {
        public string Key => "unlocks";
        private readonly IUnlockService _unlockService;
        private readonly SaveService _saveService;
        
        public UnlockSaveCoordinator(
            IUnlockService unlockService,
            SaveService saveService)
        {
            _unlockService = unlockService ?? throw new ArgumentNullException(nameof(unlockService));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
        }

        public async Task SaveAsync()
        {
            UnlockData data =
                _unlockService.CreateSnapshot();

            await _saveService.SaveAsync(
                Key,
                data);
        }

        public async Task LoadAsync()
        {
            bool exists =
                await _saveService.ExistsAsync(
                    Key);

            if (!exists)
            {
                return;
            }

            UnlockData data =
                await _saveService.LoadAsync<UnlockData>(
                    Key);

            _unlockService.Load(
                data);
        }

    }
}