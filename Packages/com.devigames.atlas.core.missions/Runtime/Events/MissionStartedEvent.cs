namespace DeviGames.Atlas.Core.Missions.Events
{
    public readonly struct MissionStartedEvent
    {
        public string MissionId { get; }

        public MissionStartedEvent(string missionId)
        {
            MissionId = missionId;
        }
    }
}