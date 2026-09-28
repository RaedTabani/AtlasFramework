using System;

using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Services;

using DeviGames.Atlas.Gameplay.Player.Interaction;
using DeviGames.Atlas.Unity.Player.Flow;

using UnityEngine;

namespace DeviGames.Playground.Player
{
    public sealed class PlaygroundPlayerInstaller :
        MonoBehaviour
    {
        [SerializeField]
        private PlayerInteractionController
            _interactionController;

        [SerializeField]
        private PlayerGameplayGate
            _gameplayGate;

        private void Awake()
        {
            if (_interactionController == null)
            {
                throw new InvalidOperationException(
                    "Player Interaction Controller is not assigned.");
            }

            if (_gameplayGate == null)
            {
                throw new InvalidOperationException(
                    "Player Gameplay Gate is not assigned.");
            }

            InteractionService interactionService =
                Services.Resolve<InteractionService>();

            IGameFlowService gameFlowService =
                Services.Resolve<IGameFlowService>();

            _interactionController.Initialize(
                interactionService);

            _gameplayGate.Initialize(
                gameFlowService);
        }
    }
}