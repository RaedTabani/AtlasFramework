namespace DeviGames.Atlas.Dev.Console.Services
{
    public sealed class DevConsoleService
    {
        public DevCommandRegistry Registry { get; } = new();

        public bool Execute(string input)
        {
            return Registry.TryExecute(input);
        }
    }
}