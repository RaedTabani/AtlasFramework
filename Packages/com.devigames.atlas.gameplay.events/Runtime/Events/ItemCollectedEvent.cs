namespace DeviGames.Atlas.Gameplay.Events
{
    public readonly struct ItemCollectedEvent
    {
        public string ItemId { get; }

        public ItemCollectedEvent(string itemId)
        {
            ItemId = itemId;
        }
    }
}