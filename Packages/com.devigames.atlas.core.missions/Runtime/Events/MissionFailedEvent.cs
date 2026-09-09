namespace DeviGames.Atlas.Core.Missions.Events
{
    public readonly struct MissionFailedEvent
    {
        public string MissionId { get; }

        public MissionFailedEvent(
            string missionId)
        {
            MissionId = missionId;
        }
    }
}