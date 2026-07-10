using DeviGames.Atlas.Core.Interaction.Interfaces;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.Events;
using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class DemoInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _succeeds = true;

        [TextArea]
        [SerializeField] private string _resultMessage = "Interaction succeeded.";

        public InteractionResult Interact(InteractionRequest request)
        {
            Debug.Log(
                $"{name} received a {request.Type} interaction from " +
                $"{request.Source.GetType().Name}.");

            EventBus.Publish(new ItemCollectedEvent("GoldenKey Found!"));
            return _succeeds
                ? InteractionResult.Successful(_resultMessage)
                : InteractionResult.Failed(_resultMessage);
        }
    }
}