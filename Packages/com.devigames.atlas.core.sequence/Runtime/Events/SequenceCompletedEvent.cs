namespace DeviGames.Atlas.Core.Sequence.Events
{
    public readonly struct SequenceCompletedEvent
    {
        public string SequenceId { get; }

        public SequenceCompletedEvent(
            string sequenceId)
        {
            SequenceId =
                sequenceId;
        }
    }
}