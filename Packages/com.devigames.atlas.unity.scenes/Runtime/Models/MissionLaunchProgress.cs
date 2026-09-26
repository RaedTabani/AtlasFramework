namespace DeviGames.Atlas.Unity.Scenes.Models
{
    public readonly struct MissionLaunchProgress
    {
        public MissionLaunchPhase Phase { get; }

        public float Progress { get; }

        public bool HasProgress { get; }

        public MissionLaunchProgress(
            MissionLaunchPhase phase,
            float progress = 0f,
            bool hasProgress = false)
        {
            Phase =
                phase;

            Progress =
                progress;

            HasProgress =
                hasProgress;
        }
    }
}