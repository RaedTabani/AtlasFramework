using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using UnityEngine;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class LoadMissionProgressStep : IBootstrapStep
    {
        private const string SaveKey = "missions";

        public string Name => "Load Mission Progress";

        public async Task ExecuteAsync(BootstrapContext context)
        {
            SaveService saveService =
                context.Services.Resolve<SaveService>();

            MissionProgressService progressService =
                context.Services.Resolve<MissionProgressService>();

            bool exists =
                await saveService.ExistsAsync(SaveKey);

            if (!exists)
            {
                Debug.Log("No mission progress save found.");
                return;
            }

            MissionProgressData data =
                await saveService.LoadAsync<MissionProgressData>(
                    SaveKey);

            progressService.LoadProgress(data);

            Debug.Log(
                $"Loaded {progressService.CompletedMissionCount} " +
                "completed missions.");
        }
    }
}