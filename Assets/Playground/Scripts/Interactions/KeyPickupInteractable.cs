using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using DeviGames.Atlas.Gameplay.Events;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class KeyPickupInteractable :
        MonoBehaviour,
        IInteractable
    {
        [SerializeField]
        private string _itemId = "golden_key";

        [SerializeField]
        private string _successMessage = "You collected the golden key.";

        [SerializeField]
        private bool _disableAfterCollection = true;

        private bool _wasCollected;

        public InteractionResult Interact(InteractionRequest request)
        {
            if (_wasCollected)
            {
                return InteractionResult.Failed(
                    "This item has already been collected.");
            }

            if (string.IsNullOrWhiteSpace(_itemId))
            {
                return InteractionResult.Failed(
                    "The pickup has no item id.");
            }

            _wasCollected = true;

            EventBus.Publish(new ItemCollectedEvent(_itemId));

            if (_disableAfterCollection)
            {
                gameObject.SetActive(false);
            }

            return InteractionResult.Successful(_successMessage);
        }
    }
}