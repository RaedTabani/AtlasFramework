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
        private readonly ISceneService _sceneService;

        public MissionLaunchService(
            MissionFlowCoordinator missionFlowCoordinator,
            IMissionCollection missionCollection,
            ISceneService sceneService)
        {
            _missionFlowCoordinator = missionFlowCoordinator ?? throw new ArgumentNullException(nameof(missionFlowCoordinator));
            _missionCollection = missionCollection ?? throw new ArgumentNullException(nameof(missionCollection));
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public async Task<bool> LaunchAsync(
            string missionId)
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
                    mission.SceneName))
            {
                Debug.LogWarning(
                    $"Mission '{missionId}' does not define a scene.");

                return false;
            }

            Debug.Log(
                $"Launching mission '{missionId}' using scene '{mission.SceneName}'.");

            if (!_missionFlowCoordinator.StartMission(
                    missionId))
            {
                Debug.LogWarning(
                    $"MissionFlowCoordinator failed to start mission '{missionId}'.");

                return false;
            }

            Debug.Log(
                "Mission flow started successfully.");

            await _sceneService.LoadAsync(
                mission.SceneName);

            return true;
        }
    }
}