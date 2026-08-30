using System;
using System.Threading.Tasks;
using UnityEngine;

using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Atlas.Unity.Scenes.Interfaces;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class MissionLaunchService
    {
        private readonly MissionFlowCoordinator _missionFlowCoordinator;
        private readonly IMissionSceneResolver _missionSceneResolver;
        private readonly ISceneService _sceneService;

        public MissionLaunchService(
            MissionFlowCoordinator missionFlowCoordinator,
            IMissionSceneResolver missionSceneResolver,
            ISceneService sceneService)
        {
            _missionFlowCoordinator = missionFlowCoordinator ?? throw new ArgumentNullException(nameof(missionFlowCoordinator));
            _missionSceneResolver = missionSceneResolver ?? throw new ArgumentNullException(nameof(missionSceneResolver));
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public async Task<bool> LaunchAsync(
            string missionId)
        {
            if (!_missionSceneResolver.TryGetSceneName(
                missionId,
                out string sceneName))
            {
                Debug.LogWarning(
                    $"No scene found for mission '{missionId}'.");

                return false;
            }

            Debug.Log(
                $"Launching mission '{missionId}' using scene '{sceneName}'.");

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
                sceneName);

            return true;
        }
    }
}