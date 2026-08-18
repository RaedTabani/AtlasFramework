using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Core.Interaction.Services;

using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class InteractionPlaygroundController :
        MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour _targetBehaviour;

        private IInteractable _target;

        private InteractionService
            _interactionService;

        public void Initialize(
            InteractionService interactionService)
        {
            _interactionService =
                interactionService;
        }

        private void Awake()
        {
            _target =
                _targetBehaviour as IInteractable;

            if (_target == null)
            {
                Debug.LogError(
                    "Assigned target does not implement " +
                    "IInteractable.");
            }
        }

        private void Update()
        {
            if (_interactionService == null ||
                _target == null)
            {
                return;
            }

            if (!Input.GetKeyDown(
                    KeyCode.E))
            {
                return;
            }

            var context =
                new InteractionContext(
                    "player");

            var request =
                new InteractionRequest(
                    _target,
                    context);

            InteractionResult result =
                _interactionService.Process(
                    request);

            Debug.Log(
                result.Succeeded
                    ? "Interaction succeeded."
                    : $"Interaction failed: {result.Reason}");
        }
    }
}