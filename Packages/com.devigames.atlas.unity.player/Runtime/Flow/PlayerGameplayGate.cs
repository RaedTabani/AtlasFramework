using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;

using UnityEngine;

namespace DeviGames.Atlas.Unity.Player.Flow
{
    public sealed class PlayerGameplayGate :
        MonoBehaviour
    {
        [SerializeField]
        private Behaviour[] _gameplayBehaviours;

        private IGameFlowService _gameFlowService;

        public void Initialize(
            IGameFlowService gameFlowService)
        {
            _gameFlowService =
                gameFlowService
                ?? throw new ArgumentNullException(
                    nameof(gameFlowService));

            Refresh();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GameFlowStateChangedEvent>(
                OnGameFlowStateChanged);
        }

        private void OnDisable()
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
            bool gameplayEnabled =
                _gameFlowService != null &&
                _gameFlowService.State ==
                GameFlowState.Gameplay;

            for (int index = 0;
                 index < _gameplayBehaviours.Length;
                 index++)
            {
                Behaviour behaviour =
                    _gameplayBehaviours[index];

                if (behaviour == null)
                {
                    continue;
                }

                behaviour.enabled =
                    gameplayEnabled;
            }
        }
    }
}