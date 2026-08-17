using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class TriggerContentData
    {
        public string Id;

        public bool Repeatable;

        public string ConditionType;

        public InventoryQuantityConditionContentData InventoryQuantity;
    }
}