using DeviGames.Atlas.Core.Interaction.Models;

namespace DeviGames.Atlas.Core.Interaction.Events
{
    public readonly struct InteractionStartedEvent
    {
        public InteractionRequest Request { get; }

        public InteractionStartedEvent(
            InteractionRequest request)
        {
            Request = request;
        }
    }
}