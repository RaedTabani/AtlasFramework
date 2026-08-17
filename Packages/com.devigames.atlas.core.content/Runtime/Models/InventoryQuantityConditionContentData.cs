using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class InventoryQuantityConditionContentData
    {
        public string ItemId;

        public int RequiredQuantity;
    }
}