using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Services;

using UnityEngine;

namespace DeviGames.Playground.MissionFlow
{
    public sealed class GameplayStateController :
        MonoBehaviour
    {
        [SerializeField]
        private Behaviour[] _controlledBehaviours;

        private IGameFlowService _gameFlowService;

        private void Start()
        {
            try
            {
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

        private void OnGameFlowStateChanged(
            GameFlowStateChangedEvent eventData)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_gameFlowService == null)
            {
                return;
            }

            bool gameplayEnabled =
                _gameFlowService.State ==
                GameFlowState.Gameplay;

            SetGameplayEnabled(
                gameplayEnabled);
        }

        private void SetGameplayEnabled(
            bool enabled)
        {
            if (_controlledBehaviours == null)
            {
                return;
            }

            foreach (Behaviour behaviour in
                _controlledBehaviours)
            {
                if (behaviour == null)
                {
                    continue;
                }

                behaviour.enabled =
                    enabled;
            }
        }
    }
}