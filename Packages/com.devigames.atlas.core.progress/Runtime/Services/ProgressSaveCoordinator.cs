using System;
using System.Linq;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Progress.Events;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Atlas.Core.Progress.Services
{
    public sealed class ProgressSaveCoordinator :
        IInitializable,
        IShutdownable
    {
        private const string SaveKey =
            "mission-progress";

        private readonly MissionProgressService
            _progressService;

        private readonly SaveService
            _saveService;

        public ProgressSaveCoordinator(
            MissionProgressService progressService,
            SaveService saveService)
        {
            _progressService =
                progressService
                ?? throw new ArgumentNullException(
                    nameof(progressService));

            _saveService =
                saveService
                ?? throw new ArgumentNullException(
                    nameof(saveService));
        }

        public void Initialize()
        {
            EventBus.Subscribe<
                MissionProgressChangedEvent>(
                    OnMissionProgressChanged);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<
                MissionProgressChangedEvent>(
                    OnMissionProgressChanged);
        }

        public async Task LoadAsync()
        {
            bool exists =
                await _saveService.ExistsAsync(
                    SaveKey);

            if (!exists)
            {
                _progressService.Restore(
                    Array.Empty<string>());

                return;
            }

            MissionProgressData data =
                await _saveService.LoadAsync<
                    MissionProgressData>(
                        SaveKey);

            if (data == null)
            {
                _progressService.Restore(data.CompletedMissionIds ?? Array.Empty<string>());

                return;
            }

            _progressService.Restore(
                data.CompletedMissionIds);
        }

        private void OnMissionProgressChanged(
            MissionProgressChangedEvent eventData)
        {
            _ = SaveAsync();
        }

        private async Task SaveAsync()
        {
            var data =
                new MissionProgressData
                {
                    CompletedMissionIds =
                        _progressService
                            .CompletedMissionIds
                            .ToArray()
                };

            await _saveService.SaveAsync(
                SaveKey,
                data);
        }
    }
}