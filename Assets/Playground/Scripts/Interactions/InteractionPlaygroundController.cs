using DeviGames.Atlas.Core.Interaction;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using DeviGames.Atlas.Core.Interaction.Services;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class InteractionPlaygroundController : MonoBehaviour
    {
        [Header("Interaction Source")]
        [SerializeField]
        private PlaygroundInteractor _interactor;

        [Header("Targets")]
        [Tooltip("Each assigned component must implement IInteractable.")]
        [SerializeField]
        private MonoBehaviour[] _targetBehaviours;

        private InteractionService _interactionService;
        private int _selectedTargetIndex;
        private bool _isInitialized;

        public void Initialize(InteractionService interactionService)
        {
            _interactionService = interactionService;
            _isInitialized = _interactionService != null;
        }

        private void Start()
        {
            SelectTarget(0);

            Debug.Log("Press 1, 2, or 3 to select a target.");
            Debug.Log("Press E to interact.");
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectTarget(0);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectTarget(1);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SelectTarget(2);

            if (Input.GetKeyDown(KeyCode.E))
                InteractWithSelectedTarget();
        }

        private void SelectTarget(int index)
        {
            if (_targetBehaviours == null ||
                index < 0 ||
                index >= _targetBehaviours.Length)
            {
                return;
            }

            _selectedTargetIndex = index;

            MonoBehaviour targetBehaviour =
                _targetBehaviours[_selectedTargetIndex];

            Debug.Log(
                targetBehaviour != null
                    ? $"Selected target: {targetBehaviour.name}"
                    : $"Target slot {index} is empty.");
        }

        private void InteractWithSelectedTarget()
        {
            if (_interactor == null)
            {
                Debug.LogError("Playground interactor is not assigned.");
                return;
            }

            if (_targetBehaviours == null ||
                _targetBehaviours.Length == 0)
            {
                Debug.LogError("No interaction targets are assigned.");
                return;
            }

            MonoBehaviour targetBehaviour =
                _targetBehaviours[_selectedTargetIndex];

            if (targetBehaviour == null)
            {
                Debug.LogWarning("Selected target is missing.");
                return;
            }

            if (targetBehaviour is not IInteractable target)
            {
                Debug.LogError(
                    $"{targetBehaviour.name} does not implement IInteractable.");

                return;
            }

            var request = new InteractionRequest(
                _interactor,
                target,
                InteractionType.Primary);

            InteractionResult result =
                _interactionService.Process(request);

            Debug.Log(
                $"Interaction result — Success: {result.Success}, " +
                $"Message: {result.Message}");
        }
    }
}