namespace DeviGames.Atlas.Core.Objectives.Models
{
    public sealed class ObjectiveState
    {
        public string ObjectiveId { get; }
        public int CurrentValue { get; private set; }
        public int TargetValue { get; }
        public bool IsStarted { get; private set; }
        public bool IsCompleted { get; private set; }

        public ObjectiveState(string objectiveId, int targetValue)
        {
            ObjectiveId = objectiveId;
            TargetValue = targetValue;
        }

        public void Start()
        {
            IsStarted = true;
        }

        public void AddProgress(int amount)
        {
            if (IsCompleted)
                return;

            CurrentValue += amount;

            if (CurrentValue >= TargetValue)
                IsCompleted = true;
        }
    }
}