namespace DeviGames.Atlas.Core.Objectives.Events
{
    public readonly struct ObjectiveCompletedEvent
    {
        public string ObjectiveId { get; }

        public ObjectiveCompletedEvent(string objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}