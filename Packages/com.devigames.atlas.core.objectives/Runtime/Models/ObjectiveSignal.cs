namespace DeviGames.Atlas.Core.Objectives.Models
{
    public readonly struct ObjectiveSignal
    {
        public string Type { get; }
        public string TargetId { get; }
        public int Amount { get; }

        public ObjectiveSignal(
            string type,
            string targetId,
            int amount = 1)
        {
            Type = type;
            TargetId = targetId;
            Amount = amount;
        }
    }
}