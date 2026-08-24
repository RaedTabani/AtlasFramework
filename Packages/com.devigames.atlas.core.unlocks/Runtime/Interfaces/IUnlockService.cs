namespace DeviGames.Atlas.Core.Unlocks.Interfaces
{
    public interface IUnlockService
    {
        bool IsUnlocked(string unlockId);

        bool Unlock(string unlockId);
    }
}