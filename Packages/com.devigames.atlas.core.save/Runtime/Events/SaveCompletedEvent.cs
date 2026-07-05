namespace DeviGames.Atlas.Core.Save.Events
{
    public readonly struct SaveCompletedEvent
    {
        public string Key { get; }

        public SaveCompletedEvent(string key)
        {
            Key = key;
        }
    }
}