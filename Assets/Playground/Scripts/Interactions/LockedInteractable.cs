using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class LockedInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _failureReason = "The object is locked.";

        public InteractionResult Interact(InteractionRequest request)
        {
            return InteractionResult.Failed(_failureReason);
        }
    }
}