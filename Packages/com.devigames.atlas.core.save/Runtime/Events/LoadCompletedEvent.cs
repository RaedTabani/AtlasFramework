namespace DeviGames.Atlas.Core.Save.Events
{
    public readonly struct LoadCompletedEvent
    {
        public string Key { get; }

        public LoadCompletedEvent(string key)
        {
            Key = key;
        }
    }
}