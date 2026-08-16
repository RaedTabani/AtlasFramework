using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class ObjectiveContentData
    {
        public string Id;

        public string DisplayName;

        public string Description;

        public int TargetValue;
    }
}