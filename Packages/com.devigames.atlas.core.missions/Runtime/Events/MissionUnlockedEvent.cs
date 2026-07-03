namespace DeviGames.Atlas.Core.Missions.Events
{
    public readonly struct MissionUnlockedEvent
    {
        public string MissionId { get; }

        public MissionUnlockedEvent(string missionId)
        {
            MissionId = missionId;
        }
    }
}