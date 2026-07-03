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
            CurrentMission = mission;

            EventBus.Publish(
                new MissionStartedEvent(mission.MissionId));
        }

        public void CompleteMission()
        {
        }

        public void AbortMission()
        {
        }
    }
}