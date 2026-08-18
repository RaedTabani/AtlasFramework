using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Core.Interaction.Services;

using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class PlayerInteractionController :
        MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera _camera;

        [Header("Interaction")]
        [SerializeField]
        private float _interactionDistance = 3f;

        [SerializeField]
        private LayerMask _interactionMask = ~0;
        

        private InteractionService _interactionService;

        public void Initialize(
            InteractionService interactionService)
        {
            _interactionService =
                interactionService;
        }

        private void Update()
        {
            if (_interactionService == null)
            {
                return;
            }

            if (!Input.GetKeyDown(
                    KeyCode.E))
            {
                return;
            }

            TryInteract();
        }

        private void TryInteract()
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
                return;
            }

            IInteractable interactable =
                FindInteractable(
                    hit.collider);

            if (interactable == null)
            {
                return;
            }

            var context =
                new InteractionContext(
                    "player");

            var request =
                new InteractionRequest(
                    interactable,
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

            foreach (MonoBehaviour behaviour
                     in behaviours)
            {
                if (behaviour is IInteractable interactable)
                {
                    return interactable;
                }
            }

            return null;
        }
    }
}