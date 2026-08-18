namespace DeviGames.Atlas.Core.Interaction.Models
{
    public readonly struct InteractionResult
    {
        public bool Succeeded { get; }

        public string Reason { get; }

        private InteractionResult(
            bool succeeded,
            string reason)
        {
            Succeeded =
                succeeded;

            Reason =
                reason;
        }

        public static InteractionResult Success()
        {
            return new InteractionResult(
                true,
                string.Empty);
        }

        public static InteractionResult Failed(
            string reason)
        {
            return new InteractionResult(
                false,
                reason ?? string.Empty);
        }
    }
}