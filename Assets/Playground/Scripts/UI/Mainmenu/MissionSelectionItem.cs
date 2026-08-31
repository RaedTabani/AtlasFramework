namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionItem
    {
        public string MissionId { get; }
        public string Title { get; }
        public bool IsUnlocked { get; }
        public bool IsCompleted { get; }

        public MissionSelectionItem(
            string missionId,
            string title,
            bool isUnlocked,
            bool isCompleted)
        {
            MissionId = missionId;
            Title = title;
            IsUnlocked = isUnlocked;
            IsCompleted = isCompleted;
        }
    }
}