using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Events;

namespace DeviGames.Atlas.Core.Missions.Services
{
    public sealed class MissionManager
    {
        public MissionDefinition CurrentMission { get; private set; }

        public bool HasActiveMission => CurrentMission != null;

        public void StartMission(MissionDefinition mission)
        {
            if(mission == null) return;

            CurrentMission = mission;

            EventBus.Publish(
                new MissionStartedEvent(mission.MissionId));
        }

        public void CompleteMission()
        {
            if (!HasActiveMission || CurrentMission == null) return;
            

            string completedId = CurrentMission.MissionId;
            CurrentMission = null; // Clean up runtime status

            EventBus.Publish(new MissionCompletedEvent(completedId));
        }

        public void AbortMission()
        {
            if (!HasActiveMission) return;

            CurrentMission = null; // Clean up runtime status
        }
    }
}