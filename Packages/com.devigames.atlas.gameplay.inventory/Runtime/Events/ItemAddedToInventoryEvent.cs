namespace DeviGames.Atlas.Gameplay.Inventory.Events
{
    public readonly struct ItemAddedToInventoryEvent
    {
        public string ItemId { get; }

        public ItemAddedToInventoryEvent(string itemId)
        {
            ItemId = itemId;
        }
    }
}