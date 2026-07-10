namespace DeviGames.Atlas.Core.Interaction.Results
{
    public readonly struct InteractionResult
    {
        public bool Success { get; }

        public string Message { get; }

        private InteractionResult(
            bool success,
            string message)
        {
            Success = success;
            Message = message;
        }

        public static InteractionResult Successful(
            string message = "")
        {
            return new InteractionResult(
                true,
                message);
        }

        public static InteractionResult Failed(
            string message)
        {
            return new InteractionResult(
                false,
                message);
        }
    }
}