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
using TMPro;
using UnityEngine.UI;

namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionController :
        MonoBehaviour
    {
        [SerializeField]
        private MissionSelectionView _view;

        [SerializeField]
        private GameObject _downloadPanel;

        [SerializeField]
        private TMP_Text _downloadText;

        [SerializeField]
        private Slider _downloadSlider;

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
                _downloadPanel.SetActive(
                    false);

                _downloadSlider.value =
                    0f;
                IProgress<float> progress =
                    new Progress<float>(
                        OnDownloadProgress);
                bool launched =
                    await _missionLaunchService
                        .LaunchAsync(
                            missionId, progress);

                if (!launched)
                {
                    Debug.LogWarning(
                        $"Mission '{missionId}' could not be launched.");
                    _downloadPanel.SetActive(
                        false);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }

        private void OnDownloadProgress(
            float progress)
        {
            if (!_downloadPanel.activeSelf)
            {
                _downloadPanel.SetActive(
                    true);
            }

            _downloadSlider.value =
                progress;

            _downloadText.text =
                $"Downloading... {progress:P0}";
        }
    }
}