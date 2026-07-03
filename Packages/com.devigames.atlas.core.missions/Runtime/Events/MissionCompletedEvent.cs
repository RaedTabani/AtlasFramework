namespace DeviGames.Atlas.Core.Missions.Events
{
    public readonly struct MissionCompletedEvent
    {
        public string MissionId { get; }

        public MissionCompletedEvent(string missionId)
        {
            MissionId = missionId;
        }
    }
}