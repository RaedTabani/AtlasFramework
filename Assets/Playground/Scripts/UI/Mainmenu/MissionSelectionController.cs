using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Unity.Application;
using DeviGames.Atlas.Unity.Scenes.Services;

using UnityEngine;

namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionController :
        MonoBehaviour
    {
        [SerializeField]
        private MissionSelectionView _view;

        private IMissionCollection _missionCollection;
        private IMissionAvailabilityService _availabilityService;
        private MissionProgressService _progressService;
        private MissionLaunchService _missionLaunchService;

        private void Start()
        {
            try
            {
                _missionCollection =
                    Services.Resolve<IMissionCollection>();

                _availabilityService =
                    Services.Resolve<IMissionAvailabilityService>();

                _progressService =
                    Services.Resolve<MissionProgressService>();

                _missionLaunchService =
                    AtlasApplication.Instance
                        .MissionLaunchService;

                Refresh();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }

        private void Refresh()
        {
            var items =
                new List<MissionSelectionItem>();

            foreach (MissionRuntime mission in
                _missionCollection.Missions){
                bool unlocked =
                    _availabilityService.IsAvailable(
                        mission.Id);

                bool completed =
                    _progressService.IsCompleted(
                        mission.Id);

                items.Add(
                    new MissionSelectionItem(
                        mission.Id,
                        mission.DisplayName,
                        unlocked,
                        completed));
            }

            _view.Show(
                items,
                PlayMission);
        }

        private async void PlayMission(
            string missionId)
        {
            try
            {
                bool launched =
                    await _missionLaunchService
                        .LaunchAsync(
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