namespace DeviGames.Atlas.Core.Missions.Events
{
    public readonly struct MissionObjectiveCompletedEvent
    {
        public string MissionId { get; }

        public string ObjectiveId { get; }

        public int CompletedObjectiveCount { get; }

        public int ObjectiveCount { get; }

        public MissionObjectiveCompletedEvent(
            string missionId,
            string objectiveId,
            int completedObjectiveCount,
            int objectiveCount)
        {
            MissionId = missionId;
            ObjectiveId = objectiveId;
            CompletedObjectiveCount =
                completedObjectiveCount;
            ObjectiveCount =
                objectiveCount;
        }
    }
}