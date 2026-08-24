using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Unlocks.Interfaces
{
    public interface IUnlockService
    {
        IReadOnlyCollection<string> UnlockedIds { get; }
        bool IsUnlocked(string unlockId);

        bool Unlock(string unlockId);
    }
}