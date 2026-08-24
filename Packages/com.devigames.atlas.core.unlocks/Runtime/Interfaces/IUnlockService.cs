using System.Collections.Generic;

using DeviGames.Atlas.Core.Unlocks.Models;

namespace DeviGames.Atlas.Core.Unlocks.Interfaces
{
    public interface IUnlockService
    {
        IReadOnlyCollection<string> UnlockedIds { get; }
        bool IsUnlocked(string unlockId);

        bool Unlock(string unlockId);
        UnlockData CreateSnapshot();
        void Load(UnlockData data);
    }
}