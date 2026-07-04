using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Services;
using UnityEngine;

public sealed class MissionDemo : MonoBehaviour
{
    [SerializeField] private MissionDefinition _mission;

    private readonly MissionService _missionService = new();

    private void OnEnable()
    {
        EventBus.Subscribe<MissionStartedEvent>(OnMissionStarted);
        EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MissionStartedEvent>(OnMissionStarted);
        EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _missionService.StartMission(_mission);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _missionService.CompleteMission();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _missionService.AbortMission();
        }
    }

    private void OnMissionStarted(MissionStartedEvent e)
    {
        Debug.Log($"Mission started: {e.MissionId}");
    }

    private void OnMissionCompleted(MissionCompletedEvent e)
    {
        Debug.Log($"Mission completed: {e.MissionId}");
    }
}