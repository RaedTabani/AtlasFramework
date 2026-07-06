using System.Linq;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Services;

namespace DeviGames.Atlas.Core.Missions.Services
{
    public sealed class MissionService : IInitializable, IShutdownable
    {
        private readonly ObjectiveService _objectiveService;

        public MissionDefinition CurrentMission { get; private set; }
        public bool HasActiveMission => CurrentMission != null;

        public MissionService(ObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Initialize()
        {
            EventBus.Subscribe<ObjectiveCompletedEvent>(OnObjectiveCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<ObjectiveCompletedEvent>(OnObjectiveCompleted);
        }

        public void StartMission(MissionDefinition mission)
        {
            if (mission == null)
                return;

            CurrentMission = mission;

            foreach (var objective in mission.Objectives)
            {
                _objectiveService.StartObjective(objective);
            }

            EventBus.Publish(new MissionStartedEvent(mission.MissionId));
        }

        public void CompleteMission()
        {
            if (CurrentMission == null)
                return;

            string missionId = CurrentMission.MissionId;

            CurrentMission = null;

            EventBus.Publish(new MissionCompletedEvent(missionId));
        }

        public void AbortMission()
        {
            CurrentMission = null;
        }

        private void OnObjectiveCompleted(ObjectiveCompletedEvent e)
        {
            if (CurrentMission == null)
                return;

            bool allCompleted = CurrentMission.Objectives.All(
                objective => _objectiveService.IsCompleted(objective.Id));

            if (allCompleted)
            {
                CompleteMission();
            }
        }
    }
}