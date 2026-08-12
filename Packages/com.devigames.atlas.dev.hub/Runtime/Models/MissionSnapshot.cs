namespace DeviGames.Atlas.Dev.Hub.Models
{
    public sealed class MissionSnapshot
    {
        public string MissionId { get; set; }

        public string DisplayName { get; set; }

        public int CompletedObjectiveCount { get; set; }

        public int ObjectiveCount { get; set; }

        public bool IsCompleted { get; set; }
    }
}