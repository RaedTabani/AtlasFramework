namespace DeviGames.Atlas.Gameplay.Inventory.Events
{
    public readonly struct ItemRemovedFromInventoryEvent
    {
        public string ItemId { get; }

        public ItemRemovedFromInventoryEvent(string itemId)
        {
            ItemId = itemId;
        }
    }
}