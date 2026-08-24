using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Models;
using DeviGames.Atlas.Core.Unlocks.Events;

namespace DeviGames.Atlas.Core.Unlocks.Services
{
    public sealed class UnlockSaveCoordinator : IInitializable, IShutdownable
    {
        private const string SaveKey =
            "unlocks";

        private readonly IUnlockService _unlockService;
        private readonly SaveService _saveService;
        
        public void Initialize()
        {
            EventBus.Subscribe<UnlockGrantedEvent>(OnUnlockGranted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<UnlockGrantedEvent>(OnUnlockGranted);
        }
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

            UnlockData data =
                await _saveService.LoadAsync<UnlockData>(
                    SaveKey);

            _unlockService.Load(
                data);
        }

        private void OnUnlockGranted(UnlockGrantedEvent eventData)
        {
            _ = SaveAsync();
        }
    }
}