namespace DeviGames.Atlas.Core.Objectives.Events
{
    public readonly struct ObjectiveStartedEvent
    {
        public string ObjectiveId { get; }

        public ObjectiveStartedEvent(string objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}