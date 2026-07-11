using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Events;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using DeviGames.Atlas.Gameplay.Inventory.Events;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using DeviGames.Playground.Interaction;
using UnityEngine;

namespace DeviGames.Playground.Application
{
    public sealed class PlaygroundApplication : MonoBehaviour
    {
        private const string MissionSaveKey = "missions";

        [Header("Content")]
        [SerializeField]
        private MissionDefinition _mission;

        [Header("Scene Components")]
        [SerializeField]
        private InteractionPlaygroundController _interactionController;

        private InteractionService _interactionService;
        private InventoryService _inventoryService;
        private ObjectiveService _objectiveService;
        private GameplayObjectiveAdapter _objectiveAdapter;
        private MissionService _missionService;
        private MissionProgressService _progressService;
        private SaveService _saveService;
        private ProgressSaveCoordinator _progressSaveCoordinator;

        private string _savePath;

        private void Awake()
        {
            CreateServices();
            InitializeServices();
            InitializeSceneComponents();
        }

        private async void Start()
        {
            await LoadExistingProgressAsync();

            if (_mission == null)
            {
                Debug.LogError(
                    "Playground mission is not assigned.");

                return;
            }

            if (_progressService.IsCompleted(_mission.MissionId))
            {
                Debug.Log(
                    $"Mission was already completed in an earlier session: " +
                    $"{_mission.MissionId}");

                return;
            }

            _missionService.StartMission(_mission);
        }

        private void OnEnable()
        {
            SubscribeToDebugEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromDebugEvents();
            ShutdownServices();
        }

        private void CreateServices()
        {
            _savePath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                "DeviGames",
                "Playground",
                "Saves");

            _interactionService = new InteractionService();
            _inventoryService = new InventoryService();
            _objectiveService = new ObjectiveService();

            _objectiveAdapter =
                new GameplayObjectiveAdapter(_objectiveService);

            _missionService =
                new MissionService(_objectiveService);

            _progressService =
                new MissionProgressService();

            _saveService =
                new SaveService(
                    new JsonFileSaveStorage(_savePath));

            _progressSaveCoordinator =
                new ProgressSaveCoordinator(
                    _progressService,
                    _saveService);
        }

        private void InitializeServices()
        {
            _inventoryService.Initialize();
            _objectiveAdapter.Initialize();
            _missionService.Initialize();
            _progressService.Initialize();
            _progressSaveCoordinator.Initialize();
        }

        private void InitializeSceneComponents()
        {
            if (_interactionController == null)
            {
                Debug.LogError(
                    "Interaction controller is not assigned.");

                return;
            }

            _interactionController.Initialize(
                _interactionService);
        }

        private async Task LoadExistingProgressAsync()
        {
            bool saveExists =
                await _saveService.ExistsAsync(MissionSaveKey);

            if (!saveExists)
            {
                Debug.Log(
                    $"No existing mission save found. Save path: {_savePath}");

                return;
            }

            MissionProgressData loaded =
                await _saveService.LoadAsync<MissionProgressData>(
                    MissionSaveKey);

            _progressService.LoadProgress(loaded);

            Debug.Log(
                $"Mission progress loaded. Completed missions: " +
                $"{_progressService.CompletedMissionCount}");
        }

        private void ShutdownServices()
        {
            _progressSaveCoordinator?.Shutdown();
            _progressService?.Shutdown();
            _missionService?.Shutdown();
            _objectiveAdapter?.Shutdown();
            _inventoryService?.Shutdown();
        }

        private void SubscribeToDebugEvents()
        {
            EventBus.Subscribe<InteractionStartedEvent>(
                OnInteractionStarted);

            EventBus.Subscribe<InteractionCompletedEvent>(
                OnInteractionCompleted);

            EventBus.Subscribe<InteractionFailedEvent>(
                OnInteractionFailed);

            EventBus.Subscribe<ItemAddedToInventoryEvent>(
                OnItemAdded);

            EventBus.Subscribe<ObjectiveProgressChangedEvent>(
                OnObjectiveProgressChanged);

            EventBus.Subscribe<ObjectiveCompletedEvent>(
                OnObjectiveCompleted);

            EventBus.Subscribe<MissionStartedEvent>(
                OnMissionStarted);

            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);

            EventBus.Subscribe<MissionProgressChangedEvent>(
                OnMissionProgressChanged);
        }

        private void UnsubscribeFromDebugEvents()
        {
            EventBus.Unsubscribe<InteractionStartedEvent>(
                OnInteractionStarted);

            EventBus.Unsubscribe<InteractionCompletedEvent>(
                OnInteractionCompleted);

            EventBus.Unsubscribe<InteractionFailedEvent>(
                OnInteractionFailed);

            EventBus.Unsubscribe<ItemAddedToInventoryEvent>(
                OnItemAdded);

            EventBus.Unsubscribe<ObjectiveProgressChangedEvent>(
                OnObjectiveProgressChanged);

            EventBus.Unsubscribe<ObjectiveCompletedEvent>(
                OnObjectiveCompleted);

            EventBus.Unsubscribe<MissionStartedEvent>(
                OnMissionStarted);

            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);

            EventBus.Unsubscribe<MissionProgressChangedEvent>(
                OnMissionProgressChanged);
        }

        private void OnInteractionStarted(
            InteractionStartedEvent e)
        {
            Debug.Log("Interaction started.");
        }

        private void OnInteractionCompleted(
            InteractionCompletedEvent e)
        {
            Debug.Log(
                $"Interaction completed: {e.Result.Message}");
        }

        private void OnInteractionFailed(
            InteractionFailedEvent e)
        {
            Debug.LogWarning(
                $"Interaction failed: {e.Result.Message}");
        }

        private void OnItemAdded(
            ItemAddedToInventoryEvent e)
        {
            Debug.Log(
                $"Inventory added item: {e.ItemId}. " +
                $"Current item count: {_inventoryService.ItemCount}");
        }

        private void OnObjectiveProgressChanged(
            ObjectiveProgressChangedEvent e)
        {
            Debug.Log(
                $"Objective progress: {e.ObjectiveId} — " +
                $"{e.CurrentValue}/{e.TargetValue}");
        }

        private void OnObjectiveCompleted(
            ObjectiveCompletedEvent e)
        {
            Debug.Log(
                $"Objective completed: {e.ObjectiveId}");
        }

        private void OnMissionStarted(
            MissionStartedEvent e)
        {
            Debug.Log(
                $"Mission started: {e.MissionId}");
        }

        private async void OnMissionCompleted(
            MissionCompletedEvent e)
        {
            Debug.Log(
                $"Mission completed: {e.MissionId}");

            // The coordinator saves asynchronously in its event handler.
            // This short delay is demo-only.
            await Task.Delay(150);

            MissionProgressData loaded =
                await _saveService.LoadAsync<MissionProgressData>(
                    MissionSaveKey);

            bool saveConfirmed =
                loaded != null &&
                loaded.IsCompleted(e.MissionId);

            Debug.Log(
                $"Save confirmed for {e.MissionId}: " +
                $"{saveConfirmed}");
        }

        private void OnMissionProgressChanged(
            MissionProgressChangedEvent e)
        {
            Debug.Log(
                $"Persistent mission progress changed: " +
                $"{e.MissionId}");
        }
    }
}