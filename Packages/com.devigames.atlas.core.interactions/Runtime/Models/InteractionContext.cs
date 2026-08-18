namespace DeviGames.Atlas.Core.Interaction.Models
{
    public readonly struct InteractionContext
    {
        public string InteractorId { get; }

        public InteractionContext(
            string interactorId)
        {
            InteractorId =
                interactorId;
        }
    }
}