namespace DeviGames.Atlas.Core.Unlocks.Events
{
    public readonly struct UnlockGrantedEvent
    {
        public string UnlockId { get; }

        public UnlockGrantedEvent(string unlockId)
        {
            UnlockId = unlockId;
        }
    }
}