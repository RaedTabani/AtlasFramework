namespace DeviGames.Atlas.Core.Execution.Models
{
    public readonly struct ExecutionContext
    {
        public float DeltaTime { get; }

        public ulong Frame { get; }

        public ExecutionContext(
            float deltaTime,
            ulong frame)
        {
            DeltaTime = deltaTime;
            Frame = frame;
        }
    }
}