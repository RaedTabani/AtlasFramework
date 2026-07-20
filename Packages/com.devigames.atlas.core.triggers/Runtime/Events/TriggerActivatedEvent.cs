namespace DeviGames.Atlas.Core.Triggers.Events
{
    public readonly struct TriggerActivatedEvent
    {
        public string TriggerId { get; }

        public TriggerActivatedEvent(
            string triggerId)
        {
            TriggerId = triggerId;
        }
    }
}