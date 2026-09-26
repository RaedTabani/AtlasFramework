using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Unity.Scenes.Models;

namespace DeviGames.Atlas.Unity.Scenes.Interfaces
{
    public interface IMissionLaunchService
    {
        Task<MissionLaunchResult> LaunchAsync(
            string missionId,
            IProgress<MissionLaunchProgress> progress = null);
    }
}