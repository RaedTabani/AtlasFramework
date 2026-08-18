using DeviGames.Atlas.Core.Interaction.Models;

namespace DeviGames.Atlas.Core.Interaction.Interfaces
{
    public interface IInteractable
    {
        string InteractionId { get; }

        bool CanInteract(
            InteractionContext context);

        InteractionResult Interact(
            InteractionContext context);
    }
}