using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;

namespace DeviGames.Atlas.Core.Interaction.Interfaces
{
    public interface IInteractable
    {
        InteractionResult Interact(
            InteractionRequest request);
    }
}