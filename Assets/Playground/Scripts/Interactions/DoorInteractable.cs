using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Gameplay.Events;

using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class DoorInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _doorId = "";

        private bool _isOpen;

        public string InteractionId =>
            _doorId;

        public bool CanInteract(
            InteractionContext context)
        {
            return !_isOpen;
        }

        public InteractionResult Interact(
            InteractionContext context)
        {
            if (_isOpen)
            {
                return InteractionResult.Failed(
                    "Door is already open.");
            }

            _isOpen = true;

            Debug.Log(
                $"Door '{_doorId}' opened.");

            EventBus.Publish(
                new DoorOpenedEvent(
                    _doorId));

            return InteractionResult.Success();
        }
    }
}