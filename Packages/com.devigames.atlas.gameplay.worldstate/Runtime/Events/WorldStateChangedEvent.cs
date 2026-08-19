namespace DeviGames.Atlas.Gameplay.WorldState.Events
{
    public readonly struct WorldStateChangedEvent
    {
        public string Key { get; }

        public bool PreviousValue { get; }

        public bool CurrentValue { get; }

        public WorldStateChangedEvent(
            string key,
            bool previousValue,
            bool currentValue)
        {
            Key =
                key;

            PreviousValue =
                previousValue;

            CurrentValue =
                currentValue;
        }
    }
}