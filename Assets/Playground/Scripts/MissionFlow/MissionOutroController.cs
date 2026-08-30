using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Services;

using UnityEngine;

namespace DeviGames.Playground.MissionFlow
{
    public sealed class MissionOutroController :
        MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;

        private MissionFlowCoordinator _missionFlowCoordinator;
        private IGameFlowService _gameFlowService;

        private void Start()
        {
            try
            {
                _missionFlowCoordinator =
                    Services.Resolve<MissionFlowCoordinator>();

                _gameFlowService =
                    Services.Resolve<IGameFlowService>();

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

        public void CompleteOutro()
        {
            if (_missionFlowCoordinator == null)
            {
                return;
            }

            if (!_missionFlowCoordinator.CompleteOutro())
            {
                Debug.LogWarning(
                    "Mission outro could not be completed.");
            }
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
                GameFlowState.MissionOutro);
        }
    }
}