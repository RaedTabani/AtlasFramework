namespace DeviGames.Atlas.Dev.Hub.Models
{
    public sealed class ObjectiveSnapshot
    {
        public string ObjectiveId { get; set; }

        public string DisplayName { get; set; }

        public int CurrentValue { get; set; }

        public int TargetValue { get; set; }

        public bool IsCompleted { get; set; }
    }
}