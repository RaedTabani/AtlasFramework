namespace DeviGames.Atlas.Dev.Console.Interfaces
{
    public interface IDevCommand
    {
        string Name { get; }
        string Description { get; }

        void Execute(string[] args);
    }
}