namespace DeviGames.Atlas.Dev.Hub.Context
{
    /// <summary>
    /// Shared context passed to every Developer Hub module.
    /// </summary>
    public sealed class DevHubContext
    {
        public RuntimeContext Runtime { get; }

        public DevHubContext()
        {
            Runtime = new RuntimeContext();
        }
    }
}