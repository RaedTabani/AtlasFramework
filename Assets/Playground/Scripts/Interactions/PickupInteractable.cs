using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Gameplay.Events;

using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class PickupInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _itemId = "";

        private bool _collected;

        public string InteractionId =>
            $"pickup:{_itemId}";

        public bool CanInteract(
            InteractionContext context)
        {
            return !_collected;
        }

        public InteractionResult Interact(
            InteractionContext context)
        {
            if (_collected)
            {
                return InteractionResult.Failed(
                    "Item has already been collected.");
            }

            _collected = true;

            EventBus.Publish(
                new ItemCollectedEvent(
                    _itemId));

            gameObject.SetActive(
                false);

            return InteractionResult.Success();
        }
    }
}