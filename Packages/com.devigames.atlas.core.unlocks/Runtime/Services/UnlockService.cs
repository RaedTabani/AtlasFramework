using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Unlocks.Events;
using DeviGames.Atlas.Core.Unlocks.Interfaces;

namespace DeviGames.Atlas.Core.Unlocks.Services
{
    public sealed class UnlockService : IUnlockService
    {
        private readonly HashSet<string> _unlockedIds =
            new(StringComparer.Ordinal);

        public IReadOnlyCollection<string> UnlockedIds => _unlockedIds;
        public bool IsUnlocked(string unlockId)
        {
            if (string.IsNullOrWhiteSpace(unlockId))
            {
                return false;
            }

            return _unlockedIds.Contains(unlockId);
        }

        public bool Unlock(string unlockId)
        {
            if (string.IsNullOrWhiteSpace(unlockId))
            {
                throw new ArgumentException(
                    "Unlock ID cannot be empty.",
                    nameof(unlockId));
            }

            if (!_unlockedIds.Add(unlockId))
            {
                return false;
            }

            EventBus.Publish(
                new UnlockGrantedEvent(unlockId));

            return true;
        }
    }
}