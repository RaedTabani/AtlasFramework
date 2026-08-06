namespace DeviGames.Atlas.Core.Objectives.Events
{
    public readonly struct ObjectiveProgressedEvent
    {
        public string ObjectiveId { get; }

        public int PreviousValue { get; }

        public int CurrentValue { get; }

        public int TargetValue { get; }

        public ObjectiveProgressedEvent(
            string objectiveId,
            int previousValue,
            int currentValue,
            int targetValue)
        {
            ObjectiveId =
                objectiveId;

            PreviousValue =
                previousValue;

            CurrentValue =
                currentValue;

            TargetValue =
                targetValue;
        }
    }
}