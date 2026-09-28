using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Models;
using DeviGames.Atlas.Gameplay.Events;

using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Inventory.Interaction
{
    public sealed class ItemPickupInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _itemId;

        public string InteractionId =>
            $"pickup.{_itemId}";

        public bool CanInteract(
            InteractionContext context)
        {
            return !string.IsNullOrWhiteSpace(
                _itemId);
        }

        public InteractionResult Interact(
            InteractionContext context)
        {
            if (string.IsNullOrWhiteSpace(
                    _itemId))
            {
                return InteractionResult.Failed(
                    "Pickup item ID is not configured.");
            }

            EventBus.Publish(
                new ItemCollectedEvent(
                    _itemId));

            gameObject.SetActive(
                false);

            return InteractionResult.Success();
        }
    }
}