using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class SuccessInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _message = "Interaction succeeded.";

        public InteractionResult Interact(InteractionRequest request)
        {
            return InteractionResult.Successful(_message);
        }
    }
}