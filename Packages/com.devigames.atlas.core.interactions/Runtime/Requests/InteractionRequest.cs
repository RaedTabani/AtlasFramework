using DeviGames.Atlas.Core.Interaction.Interfaces;

namespace DeviGames.Atlas.Core.Interaction.Requests
{
    public readonly struct InteractionRequest
    {
        public IInteractionSource Source { get; }
        
        public IInteractable Target { get; }

        public InteractionType Type { get; }

        public InteractionRequest(
            IInteractionSource source,
            IInteractable target,
            InteractionType type)
        {
            Source = source;
            Target = target;
            Type = type;
        }
    }
}