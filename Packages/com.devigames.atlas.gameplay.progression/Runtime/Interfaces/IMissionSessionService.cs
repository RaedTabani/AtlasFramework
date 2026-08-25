namespace DeviGames.Atlas.Gameplay.Progression.Interfaces
{
    public interface IMissionSessionService
    {
        string ActiveMissionId { get; }

        bool HasActiveSession { get; }

        bool Start(string missionId);

        bool Restart();

        bool Exit();
    }
}