using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class ItemCollectedObjectiveBindingData
    {
        public string ObjectiveId;
        public string ItemId;
        public int ProgressAmount = 1;
    }
}