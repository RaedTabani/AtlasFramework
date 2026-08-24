namespace DeviGames.Atlas.Gameplay.Progression.Interfaces
{
    public interface IMissionAvailabilityService
    {
        bool IsAvailable(string missionId);
    }
}