using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class RewardContentData
    {
        public string Id = "";
        public string Type = "";
        public string TargetId = "";
        public int Amount = 1;
    }
}