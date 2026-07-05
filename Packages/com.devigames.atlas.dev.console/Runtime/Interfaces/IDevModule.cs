namespace DeviGames.Atlas.Dev.Console.Interfaces
{
    public interface IDevModule
    {
        string Name { get; }

        void Register(DevHubContext context);
    }

    public class DevHubContext
    {
    }
}