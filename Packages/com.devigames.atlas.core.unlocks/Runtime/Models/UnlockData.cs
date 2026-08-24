using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Unlocks.Models
{
    [Serializable]
    public sealed class UnlockData
    {
        public int Version = 1;

        public List<string> UnlockedIds =
            new();
    }
}