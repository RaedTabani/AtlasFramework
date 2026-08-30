using System;

using DeviGames.Atlas.Unity.Application;
using DeviGames.Atlas.Unity.Scenes.Services;

using UnityEngine;

namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionController :
        MonoBehaviour
    {
        private MissionLaunchService _missionLaunchService;

        private void Start()
        {
            _missionLaunchService =
                AtlasApplication.Instance.MissionLaunchService;
        }

        public async void PlayMission(
            string missionId)
        {
            try
            {
                bool launched =
                    await _missionLaunchService.LaunchAsync(
                        missionId);

                if (!launched)
                {
                    Debug.LogWarning(
                        $"Mission '{missionId}' could not be launched.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }
    }
}