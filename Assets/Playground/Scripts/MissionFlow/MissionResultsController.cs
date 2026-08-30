using System;
using System.Threading.Tasks;
using UnityEngine;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;

using DeviGames.Atlas.Gameplay.Progression.Services;

using DeviGames.Atlas.Unity.Application;

namespace DeviGames.Playground.MissionFlow
{
    public sealed class MissionResultsController :
        MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;

        private MissionFlowCoordinator _missionFlowCoordinator;
        private IGameFlowService _gameFlowService;
        private SaveGameCoordinator _saveGameCoordinator;

        private bool _isContinuing;

        private void Start()
        {
            try
            {
                _missionFlowCoordinator =
                    Services.Resolve<MissionFlowCoordinator>();

                _gameFlowService =
                    Services.Resolve<IGameFlowService>();

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

            _isContinuing = true;

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
                _isContinuing = false;
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

            _panel.SetActive(
                _gameFlowService.State ==
                GameFlowState.MissionResults);
        }
    }
}