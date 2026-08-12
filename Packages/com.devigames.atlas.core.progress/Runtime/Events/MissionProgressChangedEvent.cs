namespace DeviGames.Atlas.Core.Progress.Events
{
    public readonly struct MissionProgressChangedEvent
    {
        public string MissionId { get; }

        public bool Completed { get; }

        public MissionProgressChangedEvent(
            string missionId,
            bool completed)
        {
            MissionId = missionId;
            Completed = completed;
        }
    }
}