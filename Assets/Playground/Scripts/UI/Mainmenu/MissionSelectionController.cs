using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Unity.Application;
using DeviGames.Atlas.Unity.Scenes.Services;

using TMPro;

using UnityEngine;
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

        [SerializeField]
        private Button _retryButton;

        private IMissionCollection _missionCollection;
        private IMissionAvailabilityService _availabilityService;
        private MissionProgressService _progressService;
        private MissionLaunchService _missionLaunchService;

        private string _pendingMissionId =
            string.Empty;

        private bool _isLaunching;

        private void Awake()
        {
            if (_view == null)
            {
                throw new InvalidOperationException(
                    "Mission selection view is not assigned.");
            }

            if (_downloadPanel == null)
            {
                throw new InvalidOperationException(
                    "Download panel is not assigned.");
            }

            if (_downloadText == null)
            {
                throw new InvalidOperationException(
                    "Download text is not assigned.");
            }

            if (_downloadSlider == null)
            {
                throw new InvalidOperationException(
                    "Download slider is not assigned.");
            }

            if (_retryButton == null)
            {
                throw new InvalidOperationException(
                    "Retry button is not assigned.");
            }

            _retryButton.onClick.AddListener(
                RetryLaunch);

            ResetDownloadUI();
        }

        private void OnDestroy()
        {
            if (_retryButton != null)
            {
                _retryButton.onClick.RemoveListener(
                    RetryLaunch);
            }
        }

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
                _missionCollection.Missions)
            {
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

        private void PlayMission(
            string missionId)
        {
            if (_isLaunching)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(
                    missionId))
            {
                Debug.LogWarning(
                    "Cannot launch a mission with an empty ID.");

                return;
            }

            LaunchMissionAsync(
                missionId);
        }

        private async void LaunchMissionAsync(
            string missionId)
        {
            if (_isLaunching)
            {
                return;
            }

            _isLaunching =
                true;

            _pendingMissionId =
                missionId;

            ResetDownloadUI();

            IProgress<float> progress =
                new Progress<float>(
                    OnDownloadProgress);

            try
            {
                bool launched =
                    await _missionLaunchService
                        .LaunchAsync(
                            missionId,
                            progress);

                if (launched)
                {
                    _pendingMissionId =
                        string.Empty;

                    return;
                }

                Debug.LogWarning(
                    $"Mission '{missionId}' could not be launched.");

                ShowLaunchFailure();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);

                ShowLaunchFailure();
            }
            finally
            {
                _isLaunching =
                    false;
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

            _retryButton.gameObject.SetActive(
                false);

            _downloadSlider.gameObject.SetActive(
                true);

            _downloadSlider.value =
                progress;

            _downloadText.text =
                $"Downloading... {progress:P0}";
        }

        private void ShowLaunchFailure()
        {
            _downloadPanel.SetActive(
                true);

            _downloadSlider.gameObject.SetActive(
                false);

            _downloadText.text =
                "Download failed.";

            _retryButton.gameObject.SetActive(
                true);
        }

        private void RetryLaunch()
        {
            if (_isLaunching)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(
                    _pendingMissionId))
            {
                return;
            }

            PlayMission(
                _pendingMissionId);
        }

        private void ResetDownloadUI()
        {
            _downloadPanel.SetActive(
                false);

            _downloadSlider.gameObject.SetActive(
                true);

            _downloadSlider.value =
                0f;

            _downloadText.text =
                string.Empty;

            _retryButton.gameObject.SetActive(
                false);
        }
    }
}