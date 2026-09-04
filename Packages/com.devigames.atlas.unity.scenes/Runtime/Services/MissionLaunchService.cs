using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Atlas.Unity.Scenes.Interfaces;

using UnityEngine;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class MissionLaunchService
    {
        private readonly MissionFlowCoordinator _missionFlowCoordinator;
        private readonly IMissionCollection _missionCollection;
        private readonly IContentDownloadService _contentDownloadService;
        private readonly ISceneService _sceneService;

        public MissionLaunchService(
            MissionFlowCoordinator missionFlowCoordinator,
            IMissionCollection missionCollection,
            IContentDownloadService contentDownloadService,
            ISceneService sceneService)
        {
            _missionFlowCoordinator = missionFlowCoordinator ?? throw new ArgumentNullException(nameof(missionFlowCoordinator));
            _missionCollection = missionCollection ?? throw new ArgumentNullException(nameof(missionCollection));
            _contentDownloadService = contentDownloadService ?? throw new ArgumentNullException(nameof(contentDownloadService));
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public async Task<bool> LaunchAsync(
            string missionId,
            IProgress<float> downloadProgress = null)
        {
            if (!_missionCollection.TryGet(
                    missionId,
                    out MissionRuntime mission))
            {
                Debug.LogWarning(
                    $"Mission '{missionId}' could not be found.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    mission.SceneKey))
            {
                Debug.LogWarning(
                    $"Mission '{missionId}' does not define a scene key.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    mission.ContentKey))
            {
                Debug.LogWarning(
                    $"Mission '{missionId}' does not define a content key.");

                return false;
            }

            Debug.Log(
                $"Preparing mission '{missionId}' using content '{mission.ContentKey}'.");

            long downloadSize =
                await _contentDownloadService.GetDownloadSizeAsync(
                    mission.ContentKey);

            Debug.Log(
                $"Mission '{missionId}' requires {downloadSize} bytes to download.");

            if (downloadSize > 0)
            {
                Debug.Log(
                    $"Downloading content '{mission.ContentKey}'.");

                await _contentDownloadService.DownloadAsync(
                    mission.ContentKey,downloadProgress);

                Debug.Log(
                    $"Content '{mission.ContentKey}' downloaded successfully.");
            }

            Debug.Log(
                $"Launching mission '{missionId}' using scene key '{mission.SceneKey}'.");

            if (!_missionFlowCoordinator.StartMission(
                    missionId))
            {
                Debug.LogWarning(
                    $"MissionFlowCoordinator failed to start mission '{missionId}'.");

                return false;
            }

            await _sceneService.LoadAsync(
                mission.SceneKey);

            return true;
        }
    }
}