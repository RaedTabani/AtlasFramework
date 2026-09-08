using System;
using System.Threading.Tasks;
using System.Text;

using TMPro;

using UnityEngine;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;

using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Models;
using DeviGames.Atlas.Gameplay.Progression.Services;

using DeviGames.Atlas.Unity.Application;

namespace DeviGames.Playground.MissionFlow
{
    public sealed class MissionResultsController :
        MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;

        [SerializeField]
        private TMP_Text _missionText;

        [SerializeField]
        private TMP_Text _statusText;
        [SerializeField]
        private TMP_Text _rewardsText;
        private MissionFlowCoordinator _missionFlowCoordinator;
        private IMissionResultService _missionResultService;
        private IGameFlowService _gameFlowService;
        private IMissionCollection _missionCollection;
        private SaveGameCoordinator _saveGameCoordinator;

        private bool _isContinuing;

        private void Start()
        {
            try
            {
                _missionFlowCoordinator =
                    Services.Resolve<MissionFlowCoordinator>();

                _missionResultService =
                    Services.Resolve<IMissionResultService>();

                _gameFlowService =
                    Services.Resolve<IGameFlowService>();
                
                _missionCollection =
                    Services.Resolve<IMissionCollection>();

                _saveGameCoordinator =
                    Services.Resolve<SaveGameCoordinator>();

                EventBus.Subscribe<GameFlowStateChangedEvent>(
                    OnGameFlowStateChanged);

                Refresh();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<GameFlowStateChangedEvent>(
                OnGameFlowStateChanged);
        }

        public async void Continue()
        {
            if (_isContinuing)
            {
                return;
            }

            _isContinuing =
                true;

            try
            {
                await CompleteResultsAsync();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
            finally
            {
                _isContinuing =
                    false;
            }
        }

        private async Task CompleteResultsAsync()
        {
            await _saveGameCoordinator.SaveAsync();

            if (!_missionFlowCoordinator.CompleteResults())
            {
                Debug.LogWarning(
                    "Mission results could not be completed.");

                return;
            }

            await AtlasApplication.Instance.ReturnToMainMenuAsync();
        }

        private void OnGameFlowStateChanged(
            GameFlowStateChangedEvent eventData)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_gameFlowService == null ||
                _panel == null)
            {
                return;
            }

            bool isResults =
                _gameFlowService.State ==
                GameFlowState.MissionResults;

            _panel.SetActive(
                isResults);

            if (!isResults)
            {
                return;
            }

            RefreshResult();
        }

        private void RefreshResult()
        {
            if (_missionText == null ||
                _statusText == null ||
                _rewardsText == null)
            {
                return;
            }

            if (!_missionResultService.TryGetResult(
                    out MissionResult result))
            {
                _missionText.text =
                    "Mission";

                _statusText.text =
                    "No mission result available.";

                _rewardsText.text =
                    string.Empty;

                Debug.LogWarning(
                    "Mission Results opened without a MissionResult.");

                return;
            }

            if (_missionCollection.TryGet(
                    result.MissionId,
                    out MissionRuntime mission))
            {
                _missionText.text =
                    mission.DisplayName;
            }
            else
            {
                _missionText.text =
                    result.MissionId;

                Debug.LogWarning(
                    $"Mission '{result.MissionId}' from MissionResult could not be found.");
            }

            _statusText.text =
                result.Completed
                    ? "Mission Complete"
                    : "Mission Failed";

            RefreshRewards(
                result);
        }

        private void RefreshRewards(
            MissionResult result)
        {
            if (result.RewardCount == 0)
            {
                _rewardsText.text =
                    "No rewards";

                return;
            }

            var builder =
                new System.Text.StringBuilder();

            builder.AppendLine(
                "Rewards");

            for (int index = 0;
                index < result.Rewards.Count;
                index++)
            {
                MissionRewardResult reward =
                    result.Rewards[index];

                builder.Append(
                    reward.Amount);

                builder.Append(
                    " × ");

                builder.Append(
                    reward.TargetId);

                if (index <
                    result.Rewards.Count - 1)
                {
                    builder.AppendLine();
                }
            }

            _rewardsText.text =
                builder.ToString();
        }
    }
}