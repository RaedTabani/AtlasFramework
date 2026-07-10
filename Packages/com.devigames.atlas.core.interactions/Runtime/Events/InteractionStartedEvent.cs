using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;

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