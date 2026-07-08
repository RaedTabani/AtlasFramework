using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Definitions;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using UnityEngine;

public sealed class AtlasVerticalSliceDemo : MonoBehaviour
{
    [Header("Demo Content")]
    [SerializeField] private MissionDefinition _mission;
    [SerializeField] private ObjectiveDefinition _objective1;
    [SerializeField] private ObjectiveDefinition _objective2;

    private ObjectiveService _objectiveService;
    private MissionService _missionService;
    private MissionProgressService _progressService;
    private ProgressSaveCoordinator _progressSaveCoordinator;
    private SaveService _saveService;

    private string _savePath;

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "AtlasDemoSaves");

        _objectiveService = new ObjectiveService();
        _missionService = new MissionService(_objectiveService);
        _progressService = new MissionProgressService();
        _saveService = new SaveService(new JsonFileSaveStorage(_savePath));

        _progressSaveCoordinator = new ProgressSaveCoordinator(
            _progressService,
            _saveService);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ObjectiveStartedEvent>(OnObjectiveStarted);
        EventBus.Subscribe<ObjectiveProgressChangedEvent>(OnObjectiveProgressChanged);
        EventBus.Subscribe<ObjectiveCompletedEvent>(OnObjectiveCompleted);
        EventBus.Subscribe<MissionStartedEvent>(OnMissionStarted);
        EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);

        _missionService.Initialize();
        _progressService.Initialize();
        _progressSaveCoordinator.Initialize();
    }

    private void Start()
    {
        Debug.Log($"Demo save path: {_savePath}");

        _missionService.StartMission(_mission);

        Debug.Log("Press 1 to progress Objective 1.");
        Debug.Log("Press 2 to progress Objective 2. It requires 3 presses.");
    }

    private void OnDisable()
    {
        _progressSaveCoordinator.Shutdown();
        _progressService.Shutdown();
        _missionService.Shutdown();

        EventBus.Unsubscribe<ObjectiveStartedEvent>(OnObjectiveStarted);
        EventBus.Unsubscribe<ObjectiveProgressChangedEvent>(OnObjectiveProgressChanged);
        EventBus.Unsubscribe<ObjectiveCompletedEvent>(OnObjectiveCompleted);
        EventBus.Unsubscribe<MissionStartedEvent>(OnMissionStarted);
        EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _objectiveService.AddProgress(_objective1.Id, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _objectiveService.AddProgress(_objective2.Id, 1);
        }
    }

    private void OnObjectiveStarted(ObjectiveStartedEvent e)
    {
        Debug.Log($"Objective started: {e.ObjectiveId}");
    }

    private void OnObjectiveProgressChanged(ObjectiveProgressChangedEvent e)
    {
        Debug.Log($"Objective progress: {e.ObjectiveId} {e.CurrentValue}/{e.TargetValue}");
    }

    private void OnObjectiveCompleted(ObjectiveCompletedEvent e)
    {
        Debug.Log($"Objective completed: {e.ObjectiveId}");
    }

    private void OnMissionStarted(MissionStartedEvent e)
    {
        Debug.Log($"Mission started: {e.MissionId}");
    }

    private async void OnMissionCompleted(MissionCompletedEvent e)
    {
        Debug.Log($"Mission completed: {e.MissionId}");

        await Task.Delay(150);

        MissionProgressData loaded =
            await _saveService.LoadAsync<MissionProgressData>("missions");

        bool saved = loaded != null && loaded.IsCompleted(e.MissionId);

        Debug.Log($"Save confirmed for {e.MissionId}: {saved}");
    }
}