using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.WorldState.Models
{
    [Serializable]
    public sealed class WorldStateData
    {
        public int Version = 1;

        public List<WorldStateEntryData>
            Entries =
                new();
    }
}