using System;

using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Core.Interaction.Services;

using DeviGames.Atlas.Gameplay.Player.Input;

using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Player.Interaction
{
    public sealed class PlayerInteractionController :
        MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private MonoBehaviour _playerInputSource;

        [Header("Interaction")]
        [SerializeField]
        private float _interactionDistance = 3f;

        [SerializeField]
        private LayerMask _interactionMask = ~0;

        private InteractionService _interactionService;
        private IPlayerInput _playerInput;

        public IInteractable CurrentTarget { get; private set; }

        public bool HasTarget =>
            CurrentTarget != null;

        public void Initialize(
            InteractionService interactionService)
        {
            _interactionService =
                interactionService
                ?? throw new ArgumentNullException(
                    nameof(interactionService));
        }

        private void OnDisable()
        {
            CurrentTarget =
                null;
        }

        private void Awake()
        {
            if (_camera == null)
            {
                throw new InvalidOperationException(
                    "Interaction Camera is not assigned.");
            }

            if (_playerInputSource == null)
            {
                throw new InvalidOperationException(
                    "Player Input Source is not assigned.");
            }

            _playerInput =
                _playerInputSource as IPlayerInput;

            if (_playerInput == null)
            {
                throw new InvalidOperationException(
                    "Player Input Source must implement IPlayerInput.");
            }
        }

        private void Update()
        {
            RefreshTarget();

            if (_interactionService == null ||
                !_playerInput.InteractPressed ||
                CurrentTarget == null)
            {
                return;
            }

            InteractWithCurrentTarget();
        }

        private void RefreshTarget()
        {
            Ray ray =
                new Ray(
                    _camera.transform.position,
                    _camera.transform.forward);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    _interactionDistance,
                    _interactionMask))
            {
                CurrentTarget =
                    null;

                return;
            }

            CurrentTarget =
                FindInteractable(
                    hit.collider);
        }

        private void InteractWithCurrentTarget()
        {
            var context =
                new InteractionContext(
                    "player");

            var request =
                new InteractionRequest(
                    CurrentTarget,
                    context);

            InteractionResult result =
                _interactionService.Process(
                    request);

            if (!result.Succeeded)
            {
                Debug.Log(
                    $"Interaction failed: {result.Reason}");
            }
        }

        private static IInteractable FindInteractable(
            Collider collider)
        {
            MonoBehaviour[] behaviours =
                collider.GetComponentsInParent<
                    MonoBehaviour>();

            for (int index = 0;
                 index < behaviours.Length;
                 index++)
            {
                if (behaviours[index] is
                    IInteractable interactable)
                {
                    return interactable;
                }
            }

            return null;
        }
    }
}