using System;
using System.Linq;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Atlas.Core.Progress.Services
{
    public sealed class ProgressSaveCoordinator :
        ISaveParticipant
    {
        private readonly MissionProgressService _progressService;
        private readonly SaveService _saveService;

        public string Key => "mission-progress";

        public ProgressSaveCoordinator(
            MissionProgressService progressService,
            SaveService saveService)
        {
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
        }

        public async Task SaveAsync()
        {
            var data =
                new MissionProgressData
                {
                    CompletedMissionIds =
                        _progressService.CompletedMissionIds.ToArray()
                };

            await _saveService.SaveAsync(Key, data);
        }

        public async Task LoadAsync()
        {
            bool exists = await _saveService.ExistsAsync(Key);

            if (!exists)
            {
                return;
            }

            MissionProgressData data =
                await _saveService.LoadAsync<MissionProgressData>(Key);

            _progressService.Restore(
                data?.CompletedMissionIds ?? Array.Empty<string>());
        }
    }
}