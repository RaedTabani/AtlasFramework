namespace DeviGames.Atlas.Core.Objectives.Events
{
    public readonly struct ObjectiveProgressChangedEvent
    {
        public string ObjectiveId { get; }
        public int CurrentValue { get; }
        public int TargetValue { get; }

        public ObjectiveProgressChangedEvent(string objectiveId, int currentValue, int targetValue)
        {
            ObjectiveId = objectiveId;
            CurrentValue = currentValue;
            TargetValue = targetValue;
        }
    }
}