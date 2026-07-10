using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction;
using DeviGames.Atlas.Core.Interaction.Events;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using DeviGames.Atlas.Core.Interaction.Services;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class InteractionPlaygroundController : MonoBehaviour
    {
        [Header("Interactor")]
        [SerializeField] private PlaygroundInteractor _interactor;

        [Header("Targets")]
        [SerializeField] private DemoInteractable[] _targets;

        private readonly InteractionService _interactionService = new();

        private int _selectedIndex;

        private void OnEnable()
        {
            EventBus.Subscribe<InteractionStartedEvent>(OnInteractionStarted);
            EventBus.Subscribe<InteractionCompletedEvent>(OnInteractionCompleted);
            EventBus.Subscribe<InteractionFailedEvent>(OnInteractionFailed);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<InteractionStartedEvent>(OnInteractionStarted);
            EventBus.Unsubscribe<InteractionCompletedEvent>(OnInteractionCompleted);
            EventBus.Unsubscribe<InteractionFailedEvent>(OnInteractionFailed);
        }

        private void Start()
        {
            SelectTarget(0);

            Debug.Log("Press 1, 2, or 3 to select a target.");
            Debug.Log("Press E to interact with the selected target.");
        }

        private void Update()
        {
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
            if (_targets == null || index < 0 || index >= _targets.Length)
                return;

            _selectedIndex = index;

            Debug.Log($"Selected target: {_targets[_selectedIndex].name}");
        }

        private void InteractWithSelectedTarget()
        {
            if (_interactor == null || _targets == null || _targets.Length == 0)
            {
                Debug.LogError("Playground interaction references are missing.");
                return;
            }

            IInteractable target = _targets[_selectedIndex];

            var request = new InteractionRequest(
                _interactor,
                target,
                InteractionType.Primary);

            InteractionResult result = _interactionService.Process(request);

            Debug.Log(
                $"Interaction result — Success: {result.Success}, " +
                $"Message: {result.Message}");
        }

        private void OnInteractionStarted(InteractionStartedEvent e)
        {
            Debug.Log("Interaction started.");
        }

        private void OnInteractionCompleted(InteractionCompletedEvent e)
        {
            Debug.Log($"Interaction completed: {e.Result.Message}");
        }

        private void OnInteractionFailed(InteractionFailedEvent e)
        {
            Debug.LogWarning($"Interaction failed: {e.Result.Message}");
        }
    }
}